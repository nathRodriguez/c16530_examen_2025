<template>
  <div class="col-lg-8 col-md-12 d-flex flex-column">
    <div class="p-4 rounded h-100 d-flex flex-column bg-white">
      <h2 class="text-center">Lista de Productos</h2>
      <ErrorMessage :message="productErrorMessage" />
      
      <ul class="list-group flex-grow-1">
        <ProductItem
          v-for="beverage in beverages"
          :key="beverage.name"
          :beverage="beverage"
          :disabled="outOfService"
          @quantity-changed="handleQuantityChange"
          @error="setErrorMessage"
        />
      </ul>
      
      <div class="d-flex align-items-center justify-content-between mt-4">
        <div>
          <p class="h5 ms-4 mb-0"><strong>Total:</strong> ₡ {{ totalCost }} colones</p>
        </div>
        <button
          class="btn btn-success px-4 py-2 me-3"
          style="font-size: 1.1rem;"
          @click="$emit('buy')"
          :disabled="outOfService"
        >
          Comprar
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import ProductItem from './ProductItem.vue';
import ErrorMessage from './ErrorMessage.vue';

export default {
  name: 'ProductList',
  components: {
    ProductItem,
    ErrorMessage
  },
  props: {
    beverages: { type: Array, required: true },
    outOfService: { type: Boolean, required: true },
    totalCost: { type: Number, required: true }
  },
  emits: ['quantity-changed', 'buy'],
  data() {
    return {
      productErrorMessage: ''
    };
  },
  methods: {
    handleQuantityChange(data) {
      this.productErrorMessage = '';
      this.$emit('quantity-changed', data);
    },
    setErrorMessage(message) {
      this.productErrorMessage = message;
    }
  }
};
</script>