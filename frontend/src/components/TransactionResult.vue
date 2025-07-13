<template>
  <div :class="['row', (transactionResult || outOfService) ? 'mt-4' : '']">
    <div class="col-12" v-if="transactionResult && !outOfService">
      <div class="alert alert-success">
        <h5 class="mb-3"><strong>¡Compra realizada exitosamente!</strong></h5>
        
        <div class="row">
          <div class="col-md-6">
            <h6><strong>Productos comprados:</strong></h6>
            <ul class="mb-2">
              <li v-for="product in transactionResult.purchasedProducts" :key="product.name">
                {{ product.quantity }} x {{ product.name }}
              </li>
            </ul>
          </div>
          
          <div class="col-md-6">
            <h6><strong>Resumen de pago:</strong></h6>
            <p class="mb-1">Total a pagar: ₡ {{ transactionResult.totalCost }}</p>
            <p class="mb-1">Total pagado: ₡ {{ transactionResult.totalPayment }}</p>
            <p class="mb-2"><strong>Su vuelto es de: ₡ {{ transactionResult.changeAmount }}</strong></p>
          </div>
        </div>

        <div v-if="transactionResult.changeBreakdown.length > 0">
          <h6><strong>Desglose del vuelto:</strong></h6>
          <ul class="mb-0">
            <li v-for="item in transactionResult.changeBreakdown" :key="item.value">
              {{ item.quantity }} moneda(s) de {{ item.value }} colones
            </li>
          </ul>
        </div>
      </div>
    </div>
    
    <div class="col-12" v-if="outOfService">
      <div class="alert alert-danger text-center">
        <h4>Fuera de servicio</h4>
        <p class="mb-0">La máquina no puede realizar más transacciones en este momento.</p>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'TransactionResult',
  props: {
    transactionResult: { type: Object, default: null },
    outOfService: { type: Boolean, required: true }
  }
};
</script>