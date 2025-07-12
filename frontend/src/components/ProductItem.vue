<template>
  <li class="list-group-item border-0">
    <div class="d-flex align-items-center justify-content-between">
      <div class="d-flex align-items-center">
        <img 
          :src="beverage.imageUrl" 
          alt="Imagen del producto" 
          class="rounded me-3" 
          style="width: 60px; height: 60px; object-fit: cover;" 
        />
        <div>
          <p class="mb-1 fw-bold">{{ beverage.name }}</p>
          <p class="mb-1">Precio: ₡ {{ beverage.price }} colones</p>
          <p class="mb-1">Cantidad: {{ beverage.quantity }} unidades</p>
        </div>
      </div>
      <QuantityControl
        :model-value="beverage.selectedQuantity"
        :max="beverage.quantity"
        :disabled="disabled"
        @update:model-value="handleQuantityChange"
        @error="$emit('error', $event)"
      />
    </div>
  </li>
</template>

<script>
import QuantityControl from './QuantityControl.vue';

export default {
  name: 'ProductItem',
  components: {
    QuantityControl
  },
  props: {
    beverage: { type: Object, required: true },
    disabled: { type: Boolean, default: false }
  },
  emits: ['quantity-changed', 'error'],
  methods: {
    handleQuantityChange(quantity) {
      this.$emit('quantity-changed', { 
        beverage: this.beverage, 
        quantity 
      });
    }
  }
};
</script>