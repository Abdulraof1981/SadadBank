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
            pageNumbers: 10,
            transDate: null,
            selectAllArray: [],
            
            dialogVisible: false,
            ruleForm: {
                desc: '',
            },
            rules: {
                desc: [
                    { required: true, message: 'الرجاء ادخال سبب الرفض', trigger: 'blur' },
                ],
            },
            bankActionIdToReject: ''
        };
    },
    
    methods: {
        ExportExcel() {
            if (this.Search === undefined || this.Search === null || this.Search === "") {
                this.Search = "";
            }
            this.$blockUI.Start();
            this.$http.GetRegistrationCSV(this.Search, this.Status, this.formatDate(this.transDate, 0), this.formatDate(this.transDate, 1))
                .then(response => {
                    this.$blockUI.Stop();

                    console.log(response.data);

                    //window.location.href = response.data;

                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    this.$message({
                        type: 'info',
                        dangerouslyUseHTMLString: true,
                        message: '<strong>' + err.response.data + '</strong>'
                    });
                    console.error(err);
                });
        },

        cahngeBankActionIdToReject(BankActionId) {
            this.bankActionIdToReject = BankActionId;
            this.ruleForm.desc = '';
            this.dialogVisible = true;
        },
        
        beforeClose() {
            this.dialogVisible = false;
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

        LastConfirmAll() {
            if (this.selectAllArray.length == 0) {
                this.$message({
                    type: 'error',
                    dangerouslyUseHTMLString: true,
                    message: '<strong>' + 'يجب تحديد الزبائن المراد إجراء تأكيد نهائي لهم أولاً' + '</strong>'
                });
                return;
            }

            for ( var i=0 ; i<this.Customers.length ; i++ ){
                if (this.Customers[i].checkbox) {
                    if (this.Customers[i].status != 2 ) {
                        this.$message({
                            type: 'error',
                            dangerouslyUseHTMLString: true,
                            message: '<strong>' + 'يجب تحديد الزبائن الذين يحتاجون إلى تأكيد نهائي فقط، الرجاء إعادة المحاولة!' + '</strong>'
                        });
                        return;
                    }
                }
            }
            
            this.$confirm('هل انت متأكد من التأكيد النهائي لجميع الحركات المحددة؟', 'تـحذير', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'لا',
                type: 'warning'
            }).then(() => {
                this.$blockUI.Start();
                this.$http.LastConfirmAll(this.selectAllArray)
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
        
        RejectCustomer() {
            this.$refs['ruleForm'].validate((valid) => {
                if (valid) {

                    this.$blockUI.Start();
                    this.$http.RejectCustomer(this.bankActionIdToReject, this.ruleForm.desc)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetCustomers(this.pageNo);
                            this.$message({
                                type: 'info',
                                dangerouslyUseHTMLString: true,
                                message: '<strong>' + 'تم رفض الحركة بنجاح' + '</strong>'
                            });
                            this.dialogVisible = false;
                        })
                        .catch((err) => {
                            this.$blockUI.Stop();
                            console.error(err);
                            this.pages = 0;
                            this.dialogVisible = false;
                        });
                
                } else {
                    console.log('error submit!!');
                    return false;
                }
            });
        },
        AddCustomers() {
            this.state = 1;
        },

        formatDate(date, i) {
            if (date == null)
                return null;

            var d = new Date(date[i]),
                month = '' + (d.getMonth() + 1),
                day = '' + d.getDate(),
                year = d.getFullYear();

            if (month.length < 2)
                month = '0' + month;
            if (day.length < 2)
                day = '0' + day;

            return [month ,day , year].join('/');
        },

        GetCustomers(pageNo) {
            this.selectAllArray = [];
            this.Customers = [];

            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            if (this.Search === undefined || this.Search === null || this.Search === "") {
                this.Search = "";
            }
            this.$blockUI.Start();
            this.$http.GetCustomers(this.pageNo, this.pageSize, this.Search, this.Status, this.formatDate(this.transDate, 0), this.formatDate(this.transDate, 1))
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
            this.Customers[index].checkbox = !this.Customers[index].checkbox;
            if ( this.Customers[index].checkbox )
                this.selectAllArray.push(this.Customers[index].bankActionId);
            else
                this.selectAllArray = this.selectAllArray.filter(item => item !== this.Customers[index].bankActionId);
        },

        changePageNumbers(pageNumbers) {
            this.pageSize = pageNumbers;
            this.GetCustomers(this.pageNo);
        }

        , resetFields() {
            this.Status= '';
            this.pageNo= 1;
            this.pages = 0;
            this.pageNumbers = 10;
            this.Search= null;
            this.transDate= null;
            this.selectAllArray = [];
            this.changePageNumbers(this.pageNumbers);
        }
    }
}