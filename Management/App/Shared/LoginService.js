import axios from 'axios';

axios.defaults.headers.common['X-CSRF-TOKEN'] = document.querySelector('meta[name="csrf-token"]').getAttribute('content');

//const baseUrl = 'https://localhost:44393/api';


export default {

    loginUserAccount(user) {       
        return axios.post(`/Security/loginUser`, user);
    },
    GetAllBranchsByBankId(BankId) {
       // axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Admin/Branch/GetAllBranchsByBankId/${BankId}`);
    },
    setClaims(BranchId) {
        return axios.get(`/Security/setClaims?BranchId=${BranchId}`);
    },

    //ResetPassword(email) {
    //    return axios.post(baseUrl + '/security/ResetPassword/' + email);
    //},   
}