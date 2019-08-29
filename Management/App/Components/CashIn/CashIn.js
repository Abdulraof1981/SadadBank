import addCashIn from './AddCashIn/AddCashIn.vue';
import moment from 'moment';

export default {
    name: 'CashIn',    
    created() {
       // this.GetBank(this.pageNo);
    },
    components: {
        'add-CashIn': addCashIn,
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
            pageNo: 1,
            pageSize: 10,
            pages: 0,  
            Banks: [],
            state: 0,
            SearchSelect: '',
            SearchText: '119830502922',
            CitizenInfo: '',
            CashIn: '',
            Logs: [],
            dialogTableVisible: false

          
        };
    },
    methods: {
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
                        this.Search(this.pageNo);
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
                        this.pages = 0;
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
