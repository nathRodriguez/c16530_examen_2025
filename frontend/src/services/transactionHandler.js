import beverageService from './beverageService';
import Swal from 'sweetalert2';

class TransactionHandler {
  async processPurchase({ beverages, paymentOptions, totalCost, totalPayment, outOfService }) {
    if (outOfService) {
      Swal.fire({
        icon: 'error',
        title: 'Fuera de servicio',
        text: 'La máquina está fuera de servicio.',
      });
      return { success: false };
    }

    const validationResult = this.validatePurchase(totalCost, totalPayment);
    if (!validationResult.valid) {
      Swal.fire({
        icon: 'error',
        title: 'Error',
        text: validationResult.message,
      });
      return { success: false };
    }

    const selectedProducts = this.extractSelectedProducts(beverages);
    const payment = this.extractPayment(paymentOptions);

    try {
      const result = await beverageService.buy(selectedProducts, payment);
      return this.handlePurchaseResult(result);
    } catch (error) {
      console.error('Purchase error:', error);
      Swal.fire({
        icon: 'error',
        title: 'Error',
        text: 'Fallo al realizar la compra',
      });
      return { success: false };
    }
  }

  validatePurchase(totalCost, totalPayment) {
    if (totalCost === 0) {
      return { valid: false, message: 'Debe seleccionar al menos un producto.' };
    }
    if (totalPayment < totalCost) {
      return { valid: false, message: 'No ha ingresado suficiente dinero para realizar la compra.' };
    }
    return { valid: true };
  }

  extractSelectedProducts(beverages) {
    return beverages
      .filter(b => b.selectedQuantity > 0)
      .map(b => ({ name: b.name, quantity: b.selectedQuantity }));
  }

  extractPayment(paymentOptions) {
    return paymentOptions
      .filter(c => c.quantity > 0)
      .map(c => ({ value: c.value, quantity: c.quantity }));
  }

  handlePurchaseResult(result) {
    if (result.status === 'error') {
      Swal.fire({
        icon: 'error',
        title: 'Error',
        text: result.message || 'Fallo al realizar la compra',
      });
      return { success: false };
    }

    if (result.status === 'out_of_service') {
      const updatedBeverages = this.updateBeverages(result.beverages);
      Swal.fire({
        icon: 'warning',
        title: 'Máquina fuera de servicio',
        text: 'La máquina está ahora fuera de servicio y no puede realizar más transacciones.',
      });
      return { 
        success: false, 
        outOfService: true, 
        updatedBeverages 
      };
    }

    const transactionResult = this.createTransactionResult(result);
    const updatedBeverages = this.updateBeverages(result.beverages);
    
    this.showSuccessAlert(transactionResult);
    
    return {
      success: true,
      transactionResult,
      updatedBeverages,
      outOfService: false
    };
  }

  updateBeverages(beverages) {
    return beverages ? beverages.map(beverage => ({
      ...beverage,
      selectedQuantity: 0
    })) : [];
  }

  createTransactionResult(result) {
    return {
      totalCost: result.totalCost,
      totalPayment: result.totalPayment,
      changeAmount: result.changeAmount,
      changeBreakdown: result.changeBreakdown || [],
      purchasedProducts: result.purchasedProducts || [],
      paymentBreakdown: result.paymentBreakdown || []
    };
  }

  showSuccessAlert(transactionResult) {
    const paymentDetails = this.formatPaymentDetails(transactionResult.paymentBreakdown);
    const changeDetails = this.formatChangeDetails(transactionResult.changeBreakdown);
    const productDetails = this.formatProductDetails(transactionResult.purchasedProducts);

    Swal.fire({
      icon: 'success',
      title: 'Compra realizada',
      html: `
        <p><b>Total a pagar:</b> ₡${transactionResult.totalCost} colones</p>
        <p><b>Total pagado:</b> ₡${transactionResult.totalPayment} colones</p>
        <p><b>Su vuelto:</b> ₡${transactionResult.changeAmount} colones</p>
        ${productDetails}
        ${paymentDetails}
        ${changeDetails}
      `,
    });
  }

  formatPaymentDetails(paymentBreakdown) {
    if (paymentBreakdown.length === 0) return '';
    
    return `<br><b>Dinero ingresado:</b><br>
      <ul style="text-align:left;display:inline-block;margin:0;padding-left:1.2em;">
        ${paymentBreakdown.map(payment => 
          `<li>${payment.quantity} ${payment.value >= 1000 ? 'billete' : 'moneda'} de ${payment.value}</li>`
        ).join('')}
      </ul>`;
  }

  formatChangeDetails(changeBreakdown) {
    if (changeBreakdown.length === 0) return '';
    
    return `<br><b>Desglose del vuelto:</b><br>
      <ul style="text-align:left;display:inline-block;margin:0;padding-left:1.2em;">
        ${changeBreakdown.map(item => 
          `<li>${item.quantity} moneda de ${item.value}</li>`
        ).join('')}
      </ul>`;
  }

  formatProductDetails(purchasedProducts) {
    if (purchasedProducts.length === 0) return '';
    
    return `<br><b>Productos comprados:</b><br>
      <ul style="text-align:left;display:inline-block;margin:0;padding-left:1.2em;">
        ${purchasedProducts.map(product => 
          `<li>${product.quantity} x ${product.name}</li>`
        ).join('')}
      </ul>`;
  }
}

export default new TransactionHandler();