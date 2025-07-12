<template>
  <div class="container mt-5">
    <h1 class="text-center mb-4">Máquina Expendedora</h1>
    <ul class="list-group">
      <li v-for="beverage in beverages" :key="beverage.name" class="list-group-item d-flex align-items-center">
        <img :src="beverage.imageUrl" alt="Imagen del refresco" class="img-fluid rounded" />
        <div class="ms-3 flex-grow-1">
          <p><strong>Nombre:</strong> {{ beverage.name }}</p>
          <p><strong>Precio:</strong> {{ beverage.price }} colones</p>
          <p><strong>Cantidad:</strong> {{ beverage.quantity }}</p>
        </div>
        <button @click="buyBeverage(beverage)" class="btn btn-primary">Comprar</button>
      </li>
    </ul>
  </div>
</template>

<script>
import beverageService from './services/beverageService';

export default {
  name: 'App',
  data() {
    return {
      beverages: [],
    };
  },
  async created() {
    try {
      this.beverages = await beverageService.getBeverages();
    } catch (error) {
      console.error('Error initializing beverages:', error);
    }
  },
  methods: {
    buyBeverage(beverage) {
      if (beverage.quantity > 0) {
        beverage.quantity -= 1;
        alert(`¡Has comprado una ${beverage.name}!`);
      } else {
        alert(`Lo sentimos, ${beverage.name} está agotado.`);
      }
    },
  },
};
</script>

<style>
img {
  width: 100px;
  height: 100px;
  object-fit: cover;
}

.list-group-item {
  display: flex;
  align-items: center;
  justify-content: space-between;
}
</style>