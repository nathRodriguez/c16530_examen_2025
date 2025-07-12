<template>
  <div class="col-lg-4 col-md-12 d-flex flex-column">
    <div class="bg-light p-4 rounded h-100 d-flex flex-column">
      <h2 class="text-center mb-4">Ingreso de Dinero</h2>
      <ErrorMessage :message="moneyErrorMessage" />
      
      <ul class="list-group flex-grow-1">
        <PaymentItem
          v-for="(coin, index) in paymentOptions"
          :key="index"
          :coin="coin"
          :disabled="outOfService"
          @quantity-changed="handlePaymentChange"
          @error="setErrorMessage"
        />
      </ul>
      
      <div class="text-center bg-light rounded mt-3">
        <p class="h5 mb-0"><strong>Total:</strong> ₡ {{ totalPayment }} colones</p>
      </div>
    </div>
  </div>
</template>

<script>
import PaymentItem from './PaymentItem.vue';
import ErrorMessage from './ErrorMessage.vue';

export default {
  name: 'PaymentPanel',
  components: {
    PaymentItem,
    ErrorMessage
  },
  props: {
    paymentOptions: { type: Array, required: true },
    outOfService: { type: Boolean, required: true },
    totalPayment: { type: Number, required: true }
  },
  emits: ['payment-changed'],
  data() {
    return {
      moneyErrorMessage: ''
    };
  },
  methods: {
    handlePaymentChange(data) {
      this.moneyErrorMessage = '';
      this.$emit('payment-changed', data);
    },
    setErrorMessage(message) {
      this.moneyErrorMessage = message;
    }
  }
};
</script>