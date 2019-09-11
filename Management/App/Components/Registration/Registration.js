import newRegister from './Register/NewRegister.vue';
import moment from 'moment';
export default {
    name: 'Registrations',    
    created() {
        this.GetCustomers(this.pageNo);
        var loginDetails = sessionStorage.getItem('currentUser');
        this.loginDetails = JSON.parse(loginDetails);
        if (loginDetails != null) {
            this.loginDetails = JSON.parse(loginDetails);
            if (this.loginDetails.userType == 5) {
                this.ColorCode = '#933c3c';
            }
        } else {
            window.location.href = '/Security/Login';
        }
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
            Status: '',
            loginDetails: {},
            pageNo: 1,
            pageSize: 10,
            pages: 0,  
            Customers: [],
            state: 0,
            Search: null,
            selectAll: false
        };
    },
    methods: {
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
                            message: '<strong> Warning: ' + response.data.message + '</strong>'
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
            this.$http.GetCustomers(this.pageNo, this.pageSize, this.Search, this.Status)
                .then(response => {
                this.$blockUI.Stop();
                this.Customers = response.data.customers;
                this.Customers.forEach(function (element) {
                    element.checkbox = false;
                });
                this.pages = response.data.count;
            })
            .catch((err) => {
                this.$blockUI.Stop();
                console.error(err);
                this.pages = 0;
            }); 
        },

        check(index) {
            /*if (index === -1) {
                this.selectAll = !this.selectAll; 
                for (var i = 0; i < this.Customers.length; i++) {
                    this.Customers[i].checkbox = true;
                }
            }
            else
            {*/
                this.Customers[index].checkbox = !this.Customers[index].checkbox;
            //}
            console.log(this.Customers);
        }


       
    }    
}
