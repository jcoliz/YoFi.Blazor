import axios from 'axios'

const client = axios.create({
    baseURL: 'http://localhost:5020/wireapi',
    json: true
  })
  
export default {
  async execute(method, resource, data) {
    return client({
      method,
      url: resource,
      data
    }).then(req => {
      return req.data
    })
  },
  listTransactions() {
    return this.execute('get', '/Transactions')
  }
}
