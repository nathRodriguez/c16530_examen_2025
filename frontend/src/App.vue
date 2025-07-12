<template>
  <div class="container-fluid mt-5">
    <h1 class="text-center mb-4">Máquina Expendedora</h1>
    <div class="card shadow-lg mx-3 mb-4 mb-lg-0">
      <div class="row">
        <ProductList 
          :beverages="beverages"
          :out-of-service="outOfService"
          :total-cost="totalCost"
          @quantity-changed="handleQuantityChange"
          @buy="handleBuy"
        />
        
        <PaymentPanel
          :payment-options="paymentOptions"
          :out-of-service="outOfService"
          :total-payment="totalPayment"
          @payment-changed="handlePaymentChange"
        />
      </div>
      
      <TransactionResult
        :transaction-result="transactionResult"
        :out-of-service="outOfService"
      />
    </div>
  </div>
</template>

<script>
import ProductList from './components/ProductList.vue';
import PaymentPanel from './components/PaymentPanel.vue';
import TransactionResult from './components/TransactionResult.vue';
import beverageService from './services/beverageService';
import transactionHandler from './services/transactionHandler';

export default {
  name: 'App',
  components: {
    ProductList,
    PaymentPanel,
    TransactionResult
  },
  data() {
    return {
      beverages: [],
      transactionResult: null,
      outOfService: false,
      paymentOptions: [
        { value: 1000, quantity: 0, imageUrl: 'https://res.cloudinary.com/dzodpkye3/image/upload/v1752287128/mil_dlhjnk.png' },
        { value: 500, quantity: 0, imageUrl: 'https://res.cloudinary.com/dzodpkye3/image/upload/v1752287131/quinientos_lo4s5n.png' },
        { value: 100, quantity: 0, imageUrl: 'https://res.cloudinary.com/dzodpkye3/image/upload/v1752287129/cien_aauvqg.png' },
        { value: 50, quantity: 0, imageUrl: 'https://res.cloudinary.com/dzodpkye3/image/upload/v1752287132/cincuenta_feicas.png' },
        { value: 25, quantity: 0, imageUrl: 'https://res.cloudinary.com/dzodpkye3/image/upload/v1752287133/veinticinco_z5hpyh.png' },
      ],
    };
  },
  computed: {
    totalCost() {
      return this.beverages.reduce((sum, beverage) => {
        const quantity = beverage.selectedQuantity || 0;
        return sum + quantity * beverage.price;
      }, 0);
    },
    totalPayment() {
      return this.paymentOptions.reduce((sum, coin) => {
        return sum + coin.value * coin.quantity;
      }, 0);
    },
  },
  async created() {
    await this.initializeBeverages();
  },
  methods: {
    async initializeBeverages() {
      try {
        const fetchedBeverages = await beverageService.getBeverages();
        this.beverages = fetchedBeverages.map(beverage => ({
          ...beverage,
          selectedQuantity: 0,
        }));
      } catch (error) {
        console.error('Error initializing beverages:', error);
        this.outOfService = true;
      }
    },
    handleQuantityChange({ beverage, quantity }) {
      beverage.selectedQuantity = quantity;
    },
    handlePaymentChange({ coin, quantity }) {
      coin.quantity = quantity;
    },
    async handleBuy() {
      const result = await transactionHandler.processPurchase({
        beverages: this.beverages,
        paymentOptions: this.paymentOptions,
        totalCost: this.totalCost,
        totalPayment: this.totalPayment,
        outOfService: this.outOfService
      });

      if (result.success) {
        this.transactionResult = result.transactionResult;
        this.beverages = result.updatedBeverages;
        this.resetPayments();
        this.outOfService = result.outOfService;
      } else if (result.outOfService) {
        this.outOfService = true;
        this.beverages = result.updatedBeverages;
        this.resetPayments();
      }
    },
    resetPayments() {
      this.paymentOptions.forEach(c => c.quantity = 0);
    }
  },
};
</script>
<style>
body {
  font-family: 'Poppins', sans-serif !important;
}

.quantity-control {
  display: flex;
  align-items: center;
}

input[type=number].no-spinner::-webkit-inner-spin-button,
input[type=number].no-spinner::-webkit-outer-spin-button {
  -webkit-appearance: none;
  margin: 0;
}

input[type=number].no-spinner {
  -moz-appearance: textfield;
}

.bg-white {
  background: #fff !important;
}
</style>