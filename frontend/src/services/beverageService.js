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
}

export default new BeverageService('https://localhost:5000');