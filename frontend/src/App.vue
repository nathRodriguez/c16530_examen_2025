<template>
  <div class="container mt-5">
    <h1 class="text-center mb-4">Máquina Expendedora</h1>
    <div v-if="errorMessage" class="alert alert-danger text-center">
      {{ errorMessage }}
    </div>
    <div class="card shadow-lg p-4">
      <ul class="list-group">
        <li v-for="beverage in beverages" :key="beverage.name" class="list-group-item border-0">
          <div class="d-flex align-items-center justify-content-between">
            <div class="d-flex align-items-center">
              <img :src="beverage.imageUrl" alt="Imagen del producto" class="rounded me-3" style="width: 80px; height: 80px; object-fit: cover;" />
              <div>
                <p class="mb-1 fw-bold">{{ beverage.name }}</p>
                <p class="mb-1">Precio: ₡ {{ beverage.price }} colones</p>
                <p class="mb-1">Cantidad: {{ beverage.quantity }} unidades</p>
              </div>
            </div>
            <div>
              <label for="quantity-{{ beverage.name }}" class="form-label visually-hidden">Cantidad:</label>
              <input
                id="quantity-{{ beverage.name }}"
                type="number"
                v-model.number="beverage.selectedQuantity"
                class="form-control text-center"
                min="0"
                :max="beverage.quantity"
                @input="validateQuantity(beverage)"
                style="width: 80px;"
              />
            </div>
          </div>
        </li>
      </ul>
    </div>
    <div class="text-center mt-4">
      <p class="h4"><strong>Costo Total:</strong> ₡ {{ totalCost }} colones</p>
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
      errorMessage: '',
    };
  },
  computed: {
    totalCost() {
      return this.beverages.reduce((sum, beverage) => {
        const quantity = beverage.selectedQuantity || 0;
        return sum + quantity * beverage.price;
      }, 0);
    },
  },
  async created() {
    try {
      const fetchedBeverages = await beverageService.getBeverages();
      this.beverages = fetchedBeverages.map(beverage => ({
        ...beverage,
        selectedQuantity: 0, // Initialize selected quantity for each beverage
      }));
    } catch (error) {
      console.error('Error initializing beverages:', error);
    }
  },
  methods: {
    validateQuantity(beverage) {
      if (beverage.selectedQuantity < 0) {
        this.errorMessage = `La cantidad de ${beverage.name} no puede ser negativa.`;
        beverage.selectedQuantity = 0;
      } else if (beverage.selectedQuantity > beverage.quantity) {
        this.errorMessage = `Lo sentimos, no hay suficientes unidades de ${beverage.name}.`;
        beverage.selectedQuantity = beverage.quantity;
      } else {
        this.errorMessage = '';
      }
    },
  },
};
</script>

<style>
body {
  font-family: 'Poppins', sans-serif !important;
}

input[type="number"]::-webkit-inner-spin-button,
input[type="number"]::-webkit-outer-spin-button {
  opacity: 1; /* Make the arrows always visible */
}
</style>