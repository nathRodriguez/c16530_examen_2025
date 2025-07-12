import axios from 'axios';

class BeverageService {
  constructor(baseUrl) {
    this.baseUrl = baseUrl;
  }

  async getBeverages() {
    try {
      const response = await axios.get(`${this.baseUrl}/api/beveragemachine/products`);
      return response.data;
    } catch (error) {
      console.error('Error fetching beverages:', error);
      throw error;
    }
  }

  async buy(products, payment) {
    try {
      const response = await axios.post(`${this.baseUrl}/api/beveragemachine/buy`, {
        products,
        payment,
      });
      return response.data;
    } catch (error) {
      if (error.response && error.response.data) {
        return error.response.data;
      }
      console.error('Error during purchase:', error);
      return { status: 'error', message: 'Fallo al realizar la compra' };
    }
  }
}

export default new BeverageService('https://localhost:5000');