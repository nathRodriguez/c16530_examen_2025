<template>
  <div class="quantity-control">
    <button 
      class="btn btn-outline-secondary btn-sm" 
      @click="decrease" 
      :disabled="disabled"
    >
      -
    </button>
    <input
      type="number"
      :value="modelValue"
      @input="handleInput"
      class="form-control text-center mx-2 no-spinner"
      style="width: 60px;"
      :disabled="disabled"
      readonly
    />
    <button 
      class="btn btn-outline-secondary btn-sm" 
      @click="increase" 
      :disabled="disabled"
    >
      +
    </button>
  </div>
</template>

<script>
export default {
  name: 'QuantityControl',
  props: {
    modelValue: { type: Number, default: 0 },
    max: { type: Number, default: Infinity },
    disabled: { type: Boolean, default: false }
  },
  emits: ['update:modelValue', 'error'],
  methods: {
    handleInput(event) {
      const value = parseInt(event.target.value) || 0;
      this.validateAndUpdate(value);
    },
    increase() {
      if (this.disabled) return;
      this.validateAndUpdate(this.modelValue + 1);
    },
    decrease() {
      if (this.disabled) return;
      if (this.modelValue > 0) {
        this.validateAndUpdate(this.modelValue - 1);
      }
    },
    validateAndUpdate(value) {
      if (value < 0 || isNaN(value)) {
        this.$emit('error', 'La cantidad no puede ser negativa.');
        this.$emit('update:modelValue', 0);
      } else if (value > this.max) {
        this.$emit('error', 'No hay suficientes unidades disponibles.');
        this.$emit('update:modelValue', this.max);
      } else {
        this.$emit('update:modelValue', value);
      }
    }
  }
};
</script>