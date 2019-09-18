
import moment from 'moment';

export default {
    name: 'CashIn',    
    created() {
       // this.GetBank(this.pageNo);
        var loginDetails = sessionStorage.getItem('currentUser');
        this.loginDetails = JSON.parse(loginDetails);
        if (loginDetails != null) {
            this.loginDetails = JSON.parse(loginDetails);
     
        } else {
            window.location.href = '/Security/Login';
        }
        this.GetCashIn(this.pageNo);
    },
 
    filters: {
        moment: function (date) {
            if (date === null) {
                return "فارغ";
            }
           // return moment(date).format('MMMM Do YYYY, h:mm:ss a');
            return moment(date).format('MMMM Do YYYY');
        }
    },
    data() {
        return {
            loginDetails: {},
            pageNo: 1,
            pageSize: 10,
            pages: 0,  
            Banks: [],
            state: 0,
            SearchSelect: '',
            SearchText: '',
            CitizenInfo: '',
            CashIn: '',
            Logs: [],
            dialogTableVisible: false,
            DateFilter: '',

            DataRenge: [],
            StartDate: null,
            EndDate: null
        };
    },
    methods: {
        ChangeDate() {
            if (this.DataRenge[0] != null) {
                this.StartDate = moment(this.DataRenge[0]).format('YYYY-MM-DD');
                this.EndDate = moment(this.DataRenge[1]).format('YYYY-MM-DD');
                //this.GetDeath();
            }
        },

        LastConfirm(CashInId) {
            //LastConfirm
            this.$confirm('سيؤدي ذلك إلى التأكيد النهائي للعملية. استمر؟', 'تـحذير', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'لا',
                type: 'warning'
            }).then(() => {
                this.$http.LastConfirmCashIn(CashInId)
                    .then(response => {
                        this.$blockUI.Stop();
                        this.refreshOrGet();
                        this.$message({
                            type: 'info',
                            dangerouslyUseHTMLString: true,
                            message: '<strong>' + 'تم التأكيد النهائي للعملية' + '</strong>'
                        });
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
            });
        },


        Refresh() {
            this.DateFilter = '';
            this.CitizenInfo = '';
            this.SerachText = '';
            this.SearchSelect = '';
           // this.CashIn = '';
            this.GetCashIn(this.pageNo);
        },

        RejectCashIn(CashInId) {
            this.$confirm('سيؤدي ذلك إلى رفض العملية. استمر؟', 'تـحذير', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'لا',
                type: 'warning'
            }).then(() => {
                this.$blockUI.Start();
                this.$http.RemoveCashIn(CashInId)
                    .then(response => {
                        this.$blockUI.Stop();
                        this.refreshOrGet();
                        this.$message({
                            type: 'info',
                            dangerouslyUseHTMLString: true,
                            message: '<strong>' + 'تم رفض العملية بنجاح' + '</strong>'
                        });
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
            });
        },

        ShowLogs(CashInId) {
            this.dialogTableVisible = true;
            this.$blockUI.Start();
            this.$http.UserActions(CashInId)
                .then(response => {
                    this.$blockUI.Stop();
                    this.Logs = response.data.logs;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.$message({
                        type: 'error',
                        dangerouslyUseHTMLString: true,
                        message: '<strong>' + err.response.data + '</strong>'
                    });
                    this.pages = 0;
                });
        },


        GetCashIn(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            this.$blockUI.Start();
            this.$http.GetCashIn(this.pageNo,this.pageSize)
                .then(response => {
                    this.$blockUI.Stop();
                  //  this.CitizenInfo = response.data.personalInfo;
                    this.CashIn = response.data.cashIn;
                    //this.Banks = response.data.banks;
                    this.pages = response.data.count;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.$message({
                        type: 'error',
                        dangerouslyUseHTMLString: true,
                        message: '<strong>' + err.response.data + '</strong>'
                    });
                    this.pages = 0;
                });
        },

        refreshOrGet() {
            if (this.CitizenInfo == '') {
                this.GetCashIn(this.pageNo);
            } else {
                this.Search();
            }
        },

        Search() {
            if (this.SearchSelect == 1) {
                // nid rouls
                if (!this.SearchText) {
                    this.$message({
                        type: 'error',
                        dangerouslyUseHTMLString: true,
                        message: '<strong>الرجاء ادخال الرقم الوطني</strong>'
                    });
                    return;
                }
                if (this.SearchText.length < 12) {
                    this.$message({
                        type: 'error',
                        dangerouslyUseHTMLString: true,
                        message: '<strong>الرجاء ادخال الرقم الوطني بطريقة الصحيحه</strong>'
                    });
                    return;
                }

            }

            if (this.SearchSelect == 2) {
                //MSISDN rouls
                if (!this.SearchText) {
                    this.$message({
                        type: 'error',
                        dangerouslyUseHTMLString: true,
                        message: '<strong>'+'الرجاء ادخال رقم الهاتف'+'</strong>'
                    });
                    return;
                }

                if (this.SearchText.length < 9) {
                    this.$message({
                        type: 'error',
                        dangerouslyUseHTMLString: true,
                        message: '<strong>' + 'الرجاء ادخال رقم الهاتف بطريقة الصحيحه' + '</strong>'
                    });
                    return;
                }

            }


            this.$blockUI.Start();
            this.$http.SearchBy(this.SearchText,this.SearchSelect)
                .then(response => {
                    this.$blockUI.Stop();
                    this.CitizenInfo = response.data.personalInfo;
                    this.CashIn = response.data.cashIn;
                    //this.Banks = response.data.banks;
                    //this.pages = response.data.count;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.$message({
                        type: 'error',
                        dangerouslyUseHTMLString: true,
                        message: '<strong>' + err.response.data + '</strong>'
                    });
                    this.pages = 0;
                });
        },
        

        AddCashIn() {
            this.state = 1;
        }
       
    }    
}
