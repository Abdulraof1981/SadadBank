import addBank from './AddBank/AddBank.vue';
import moment from 'moment';
export default {
    name: 'Banks',    
    created() {
        var loginDetails = sessionStorage.getItem('currentUser');
        this.loginDetails = JSON.parse(loginDetails);
        if (loginDetails != null) {
            this.loginDetails = JSON.parse(loginDetails);
            if (this.loginDetails.userType != 1) {
                window.location.href = '/Security/Login';
            }
        } else {
            window.location.href = '/Security/Login';
        }

        this.GetBank(this.pageNo);

    },
    components: {
        'add-Bank': addBank,
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
            state:0,
          
        };
    },
    methods: {
        RemoveBank(BankId) {
            this.$confirm('سيؤدي ذلك إلى حذف المصرف نهائيا. استمر؟', 'تـحذير', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'لا',
                type: 'warning'
            }).then(() => {
                this.$blockUI.Start();
                this.$http.RemoveBank(BankId)
                    .then(response => {
                        this.$blockUI.Stop();
                        this.GetBank(this.pageNo);
                        this.$message({
                            type: 'info',
                            dangerouslyUseHTMLString: true,
                            message: '<strong>' + 'تم مسح المصرف بنجاح' + '</strong>'
                        });
                    })
                    .catch((err) => {
                        this.$blockUI.Stop();
                        console.error(err);
                        this.pages = 0;
                    });
            });
        },   
        AddBanks() {
            this.state = 1;
        },
        GetBank(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            this.$blockUI.Start();
            this.$http.GetBank(this.pageNo, this.pageSize)
                .then(response => {
                    this.$blockUI.Stop();
                    this.Banks = response.data.banks;
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
