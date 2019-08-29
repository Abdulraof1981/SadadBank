import newRegister from './Register/NewRegister.vue';
import moment from 'moment';
export default {
    name: 'Registrations',    
    created() {
        this.GetCustomers(this.pageNo);
        this.GetCurrentUserType();
    },
    components: {
        'New-Register': newRegister,
    },
    filters: {
        moment: function (date) {
            if (date === null) {
                return "فارغ";
            }
           // return moment(date).format('MMMM Do YYYY, h:mm:ss a');
            return moment(date).format('MMMM Do YYYY');
        },
        moment2: function (date) {
            if (date === null) {
                return "فارغ";
            }
            return moment(date).format('MMMM Do YYYY, h:mm:ss a');
            //return moment(date).format('MMMM Do YYYY');
        }
    },
    data() {
        return {
            pageNo: 1,
            pageSize: 10,
            pages: 0,  
            Customers: [],
            state: 0,
            Search: null,
            userType: 0
        };
    },
    methods: {
        GetCurrentUserType() {
            this.$http.GetCurrentUserType().then(response => {
                this.userType = response.data.userType;
            }).catch((err) => {
                console.error(err);
            });
        },

        LastConfirm(BankActionId) {
            this.$confirm('هل انت متأكد من التأكيد النهائي لهذه الحركة؟', 'تـحذير', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'لا',
                type: 'warning'
            }).then(() => {
                this.$blockUI.Start();
                this.$http.LastConfirm(BankActionId)
                .then(response => {
                    this.$blockUI.Stop();
                    this.GetCustomers(this.pageNo);
                    if (response.data.code == 0) {
                        this.$message({
                            type: 'info',
                            dangerouslyUseHTMLString: true,
                            message: '<strong>' + response.data.message + '</strong>'
                        });
                    } else if (response.data.code == 1) {
                        this.$message({
                            type: 'info',
                            dangerouslyUseHTMLString: true,
                            message: '<strong> خطأ: ' + response.data.message + '</strong>'
                        });
                    } else {
                        this.$message({
                            type: 'error',
                            dangerouslyUseHTMLString: true,
                            message: '<strong>' + response.data.message + '</strong>'
                        });
                    }
                }).catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.pages = 0;
                });
            });
        },

        RejectCustomer(BankActionId) {
            this.$confirm('هل تريد رفض هذه الحركة؟', 'تـحذير', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'لا',
                type: 'warning'
            }).then(() => {
                this.$blockUI.Start();
                this.$http.RejectCustomer(BankActionId)
                .then(response => {
                    this.$blockUI.Stop();
                    this.GetCustomers(this.pageNo);
                    this.$message({
                        type: 'info',
                        dangerouslyUseHTMLString: true,
                        message: '<strong>' + 'تم رفض الحركة بنجاح' + '</strong>'
                    });
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.pages = 0;
                });
            });
        },
        AddCustomers() {
            this.state = 1;
        },
        GetCustomers(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            if (this.Search === undefined || this.Search === null || this.Search === "") {
                this.Search = "";
            }
            this.$blockUI.Start();
            this.$http.GetCustomers(this.pageNo, this.pageSize, this.Search)
                .then(response => {
                this.$blockUI.Stop();
                this.Customers = response.data.customers;
                console.log(this.Customers);
                this.pages = response.data.count;
            })
            .catch((err) => {
                this.$blockUI.Stop();
                console.error(err);
                this.pages = 0;
            });
        },


       
    }    
}
