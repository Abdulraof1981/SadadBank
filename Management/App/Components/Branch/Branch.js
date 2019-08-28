import addBranch from './AddBranch/AddBranch.vue';
import moment from 'moment';
export default {
    name: 'Branchs',    
    created() {
        this.GetAllBank();
        this.GetBranch(this.pageNo);
    },
    components: {
        'add-Branch': addBranch,
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
            Branchs:[],
            state: 0,
            SelectedBank: ''
          
        };
    },
    methods: {
        RemoveBranch(BranchId) {
            this.$confirm('سيؤدي ذلك إلى حذف الفرع المصرفي نهائيا. استمر؟', 'تـحذير', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'لا',
                type: 'warning'
            }).then(() => {
                this.$blockUI.Start();
                this.$http.RemoveBranch(BranchId)
                    .then(response => {
                        this.$blockUI.Stop();
                        this.GetBranch(this.pageNo);
                        this.$message({
                            type: 'info',
                            dangerouslyUseHTMLString: true,
                            message: '<strong>' + 'تم مسح الفرع المصرفي بنجاح' + '</strong>'
                        });
                    })
                    .catch((err) => {
                        this.$blockUI.Stop();
                        console.error(err);
                        this.pages = 0;
                    });
            });
        },   

        AddBranch() {
            this.state = 1;
        },

        FilterBranch() {
            this.GetBranch(this.pageNo);
        },

        GetBranch(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            this.$blockUI.Start();
            this.$http.GetBranch(this.pageNo, this.pageSize, this.SelectedBank)
                .then(response => {
                    this.$blockUI.Stop();
                    this.Branchs = response.data.branchs;
                    this.pages = response.data.count;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.pages = 0;
                });
        },


        GetAllBank() {
            this.$blockUI.Start();
            this.$http.GetAllBank()
                .then(response => {
                    this.$blockUI.Stop();
                    this.Banks = response.data.banks;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.pages = 0;
                });
        },


       
    }    
}
