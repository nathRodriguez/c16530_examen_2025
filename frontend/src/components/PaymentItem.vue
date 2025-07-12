<template>
  <li class="list-group-item border-0 bg-transparent">
    <div class="d-flex align-items-center justify-content-between">
      <div class="d-flex align-items-center">
        <img 
          :src="coin.imageUrl" 
          alt="Imagen de moneda/billete" 
          class="rounded me-3" 
          style="width: 60px; height: 60px; object-fit: cover;" 
        />
        <div>
          <p class="mb-1 fw-bold">
            {{ coin.value === 1000 ? 'Billete' : 'Moneda' }} de ₡ {{ coin.value }}
          </p>
        </div>
      </div>
      <QuantityControl
        :model-value="coin.quantity"
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
  name: 'PaymentItem',
  components: {
    QuantityControl
  },
  props: {
    coin: { type: Object, required: true },
    disabled: { type: Boolean, default: false }
  },
  emits: ['quantity-changed', 'error'],
  methods: {
    handleQuantityChange(quantity) {
      this.$emit('quantity-changed', { 
        coin: this.coin, 
        quantity 
      });
    }
  }
};
</script>