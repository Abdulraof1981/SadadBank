import axios from 'axios';
axios.defaults.headers.common['X-CSRF-TOKEN'] = document.querySelector('meta[name="csrf-token"]').getAttribute('content');
export default {
    //Login(loginName, password, secretNo) {
    //    return axios.post(baseUrl + '/security/login', { loginName, password, secretNo });
    //},
    //Logout() {
    //    return axios.post(baseUrl + '/security/logout');
    //},    
    //CheckLoginStatus() {
    //    return axios.post('/security/checkloginstatus');
    //}, 
    // ******************** Users *********************************
    GetUsers(pageNo, pageSize, UserType, BranchId) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Admin/User/Get?pageno=${pageNo}&pagesize=${pageSize}&UserType=${UserType}&BranchId=${BranchId}`);
    },
    AddUser(User) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post('/Api/Admin/User/AddUser', User);
    },
    RemoveUser(UserId) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post(`/Api/Admin/User/${UserId}/delete`);
    },
    //GetStatistic
    GetStatistic() {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Admin/User/GetStatistic`);
    },

    ActivateUser(Status, UserId) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post(`/Api/Admin/User/${Status}/${UserId}/Activate`);
    },

    UserActions(CashInId) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post(`/Api/Admin/User/${CashInId}/UserActions`);
    },

    GetUserData(userId){
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Admin/User/GetData?userId=${userId}`);
    },

    EditUser(User) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post('/Api/Admin/User/EditUser', User);
    },
    
    // **************************** Bank *****************************
    GetBank(pageNo, pageSize) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Admin/Bank/Get?pageno=${pageNo}&pagesize=${pageSize}`);
    },
    GetAllBank() {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Admin/Bank/GetAllBank`);
    },
    AddBank(Bank) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post(`/Api/Admin/Bank/Add`, Bank);
    },
    RemoveBank(BankId) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post(`/Api/Admin/Bank/${BankId}/delete`);
    },
    // ************************ Branch ****************************
    GetBranch(pageNo, pageSize, BankId) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Admin/Branch/Get?pageno=${pageNo}&pagesize=${pageSize}&BankId=${BankId}`);
    },
    GetAllBranchsByBankId(BankId) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Admin/Branch/GetAllBranchsByBankId/${BankId}`);
    },
    AddBranch(Branch) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post(`/Api/Admin/Branch/Add`, Branch);
    },
    RemoveBranch(BranchId) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post(`/Api/Admin/Branch/${BranchId}/delete`);
    },
    GetAllBranch() {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Admin/Branch/GetAllBranch`);
    },
    // ********************** Personal Info *******************************

    //SearchBy
    SearchBy(Digits, SearchType) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Admin/CashIn/SearchBy/${Digits}/${SearchType}`);
    },
    AddCashIn(CashIn) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post(`/Api/Admin/CashIn/Add`, CashIn);
    },
    RemoveCashIn(CashInId) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post(`/Api/Admin/CashIn/${CashInId}/delete`);
    },
    GetCashIn(pageNo, pageSize) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Admin/CashIn/Get?pageno=${pageNo}&pagesize=${pageSize}`);
    },
    LastConfirmCashIn(CashInId) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post(`/Api/Admin/CashIn/${CashInId}/Confirm`);
    },

   




    //********************* Registration Service *****************************
    GetCustomers(pageNo, pageSize, Search, Status) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Admin/Registration/Get?pageno=${pageNo}&pagesize=${pageSize}&search=${Search}&status=${Status}`);
    },
    RejectCustomer(BankActionId) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post(`/Api/Admin/Registration/${BankActionId}/Reject`);
    },
    LastConfirm(BankActionId) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post(`/Api/Admin/Registration/${BankActionId}/LastConfirm`);
    },
    getNidInfo(nid) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Admin/Registration/getNidInfo?nid=${nid}`);
    },
    getPhoneInfo(phone) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Admin/Registration/getPhoneInfo?phone=${phone}`);
    },
    GetCurrentUserType() {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Admin/Registration/GetCurrentUserType`);
    },
    GetAllCities() {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Admin/Registration/GetAllCities`);
    },
    AddCustomer(ob) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post(`/Api/Admin/Registration/Add`, ob);
    },

    // ********************************** CashIn **************************************



}