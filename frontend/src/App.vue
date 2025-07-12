<template>
  <div class="container-fluid mt-5">
    <h1 class="text-center mb-4">Máquina Expendedora</h1>
    <div class="card shadow-lg mx-3 mb-4 mb-lg-0">
      <div class="row" style="min-height: 600px;">
        <!-- Productos -->
        <div class="col-lg-8 col-md-12 d-flex flex-column">
          <div class="p-4 rounded h-100 d-flex flex-column bg-white">
            <h2 class="text-center">Lista de Productos</h2>
            <div v-if="productErrorMessage" class="alert alert-danger text-center py-2">
              {{ productErrorMessage }}
            </div>
            <ul class="list-group flex-grow-1">
              <li v-for="beverage in beverages" :key="beverage.name" class="list-group-item border-0">
                <div class="d-flex align-items-center justify-content-between">
                  <div class="d-flex align-items-center">
                    <img :src="beverage.imageUrl" alt="Imagen del producto" class="rounded me-3" style="width: 60px; height: 60px; object-fit: cover;" />
                    <div>
                      <p class="mb-1 fw-bold">{{ beverage.name }}</p>
                      <p class="mb-1">Precio: ₡ {{ beverage.price }} colones</p>
                      <p class="mb-1">Cantidad: {{ beverage.quantity }} unidades</p>
                    </div>
                  </div>
                  <div class="quantity-control">
                    <button class="btn btn-outline-secondary btn-sm" @click="decreaseQuantity(beverage)">-</button>
                    <input
                      type="number"
                      v-model.number="beverage.selectedQuantity"
                      @input="validateQuantity(beverage)"
                      class="form-control text-center mx-2 no-spinner"
                      style="width: 60px;"
                    />
                    <button class="btn btn-outline-secondary btn-sm" @click="increaseQuantity(beverage)">+</button>
                  </div>
                </div>
              </li>
            </ul>
            <div class="text-center">
              <p class="h5"><strong>Total:</strong> ₡ {{ totalCost }} colones</p>
            </div>
          </div>
        </div>
        <!-- Dinero -->
        <div class="col-lg-4 col-md-12 d-flex flex-column">
          <div class="bg-light p-4 rounded h-100 d-flex flex-column">
            <h2 class="text-center mb-4">Ingreso de Dinero</h2>
            <div v-if="moneyErrorMessage" class="alert alert-danger text-center py-2">
              {{ moneyErrorMessage }}
            </div>
            <ul class="list-group flex-grow-1">
              <li v-for="(coin, index) in paymentOptions" :key="index" class="list-group-item border-0 bg-transparent">
                <div class="d-flex align-items-center justify-content-between">
                  <div class="d-flex align-items-center">
                    <img :src="coin.imageUrl || 'https://via.placeholder.com/60'" alt="Imagen de moneda/billete" class="rounded me-3" style="width: 60px; height: 60px; object-fit: cover;" />
                    <div>
                      <p class="mb-1 fw-bold">
                        {{ coin.value === 1000 ? 'Billete' : 'Moneda' }} de ₡ {{ coin.value }}
                      </p>
                    </div>
                  </div>
                  <div class="quantity-control">
                    <button class="btn btn-outline-secondary btn-sm" @click="decreasePayment(coin)">-</button>
                    <input
                      type="number"
                      v-model.number="coin.quantity"
                      @input="validatePayment(coin)"
                      class="form-control text-center mx-2 no-spinner"
                      style="width: 60px;"
                    />
                    <button class="btn btn-outline-secondary btn-sm" @click="increasePayment(coin)">+</button>
                  </div>
                </div>
              </li>
            </ul>
            <div class="text-center bg-light rounded">
              <p class="h5"><strong>Total:</strong> ₡ {{ totalPayment }} colones</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import beverageService from './services/beverageService';

export default {
  name: 'App',
  data() {
    return {
      beverages: [],
      productErrorMessage: '',
      moneyErrorMessage: '',
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
    try {
      const fetchedBeverages = await beverageService.getBeverages();
      this.beverages = fetchedBeverages.map(beverage => ({
        ...beverage,
        selectedQuantity: 0,
      }));
    } catch (error) {
      console.error('Error initializing beverages:', error);
    }
  },
  methods: {
    validateQuantity(beverage) {
      if (beverage.selectedQuantity < 0 || isNaN(beverage.selectedQuantity)) {
        this.productErrorMessage = `La cantidad de ${beverage.name} no puede ser negativa.`;
        beverage.selectedQuantity = 0;
      } else if (beverage.selectedQuantity > beverage.quantity) {
        this.productErrorMessage = `Lo sentimos, no hay suficientes unidades de ${beverage.name}.`;
        beverage.selectedQuantity = beverage.quantity;
      } else {
        this.productErrorMessage = '';
      }
    },
    validatePayment(coin) {
      if (coin.quantity < 0 || isNaN(coin.quantity)) {
        this.moneyErrorMessage = `La cantidad de monedas/billetes de ₡ ${coin.value} no puede ser negativa.`;
        coin.quantity = 0;
      } else {
        this.moneyErrorMessage = '';
      }
    },
    increaseQuantity(beverage) {
      if (beverage.selectedQuantity < beverage.quantity) {
        beverage.selectedQuantity++;
        this.productErrorMessage = '';
      } else {
        this.productErrorMessage = `Lo sentimos, no hay suficientes unidades de ${beverage.name}.`;
      }
    },
    decreaseQuantity(beverage) {
      if (beverage.selectedQuantity > 0) {
        beverage.selectedQuantity--;
        this.productErrorMessage = '';
      }
    },
    increasePayment(coin) {
      coin.quantity++;
      this.moneyErrorMessage = '';
    },
    decreasePayment(coin) {
      if (coin.quantity > 0) {
        coin.quantity--;
        this.moneyErrorMessage = '';
      }
    },
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