export default {
    name: 'login',
    created() {
        this.returnurl = location.pathname;
        this.SetRules();

    },
    data() {
        return {
            userInfo: {},
            IsMultiPermissions: false,
            returnurl: '',
            form: {
                Password: null,
                Email: null
            },
            form2: {
                Email: null
            },
            Branchs: [],
            success: { confirmButtonText: 'OK', type: 'success', dangerouslyUseHTMLString: true, center: true },
            error: { confirmButtonText: 'OK', type: 'error', dangerouslyUseHTMLString: true, center: true },
            warning: { confirmButtonText: 'OK', type: 'warning', dangerouslyUseHTMLString: true, center: true },
            rules: {},
            forgetPassowrd: false,
            SelectedBranch: '',


        };
    },
    methods: {
        //Login() {
        //    this.$emit('authenticated');
        //},

        GoToMainBranch() {
            location.href = "/";
        },

        SelectBranch() {

            let $blockUI = this.$loading({
                fullscreen: true,
                text: 'loading ...'
            });
            this.$http.setClaims(this.SelectedBranch)
                .then(response => {
                    $blockUI.close();
                    console.log(response);
                    this.Branchs = response.data.branchs;
                    var userInfo = JSON.parse(sessionStorage.getItem('currentUser'));
                    userInfo.branchId = this.SelectedBranch;
                    userInfo.branchName = this.userInfo.userBranch.find(x => x.branchId == this.SelectedBranch).name;
                    sessionStorage.setItem('currentUser', {});
                    sessionStorage.setItem('currentUser', JSON.stringify(userInfo));
                    location.href = "/";
                })
                .catch((err) => {
                    $blockUI.close();
                    console.error(err);
                    this.pages = 0;
                });
        },

        Save() {
            if (!this.form.Email) {
                this.$alert('<h4>' + 'الرجاء إدخال البريد الإلكتروني او اسم المستخدم' + '</h4>', '', this.warning);
                return;
            }

            if (!this.form.Password) {
                this.$alert('<h4>' + 'الرجاء إدخال الرقم السري' + '</h4>', '', this.warning);
                return;
            }

            let $blockUI = this.$loading({
                fullscreen: true,
                text: 'loading ...'
            });
            this.$http.loginUserAccount(this.form)
                .then(response => {
                   
                    $blockUI.close();    
                    sessionStorage.setItem('currentUser', JSON.stringify(response.data));
                    var userInfo = JSON.parse(sessionStorage.getItem('currentUser'));
                    this.userInfo = JSON.parse(sessionStorage.getItem('currentUser'));      
                    if (userInfo.userType == 1) {
                        location.href = "/";
                    } else {
                        if (userInfo.userBranch.length <= 0) {
                            location.href = "/";
                        } else {
                            // open select
                            console.log("userInfo.bankId");
                            console.log(userInfo.bankId);
                            //this.GetBranchs(userInfo.bankId);
                            this.IsMultiPermissions = true;

                        }
                    }
                })
                .catch((error) => {
                    $blockUI.close()
                    this.$alert('<h4>' + error.response.data + '</h4>', '', this.warning);
                    if (error.response.status == 400) {
                        this.$alert('<h4>' + error.response.data + '</h4>', '', this.warning);
                    } else if (error.response.status == 404) {
                        this.$alert('<h4>' + error.response.data + '</h4>', '', this.error);
                    } else {
                        console.log(error.response);
                    }
                });
        },

        GetBranchs(Id) {
            //GetAllBranchsByBankId
            console.log(Id + "/" + Id);
            let $blockUI = this.$loading({
                fullscreen: true,
                text: 'loading ...'
            });
            this.$http.GetAllBranchsByBankId(Id)
                .then(response => {
                    $blockUI.close(); 
                    console.log(response);
                    this.Branchs = response.data.branchs;
                })
                .catch((err) => {
                    $blockUI.close(); 
                    console.error(err);
                    this.pages = 0;
                });
        },


        ResetPassword(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    let $blockUI = this.$loading({
                        fullscreen: true,
                        text: 'loading ...'
                    });
                    this.$http.ResetPassword(this.form2.Email.trim())
                    .then(response => {
                        $blockUI.close()                        
                        this.form2.Email = null;
                        this.forgetPassowrd = false;
                        this.$alert('<h4>' + response.data + '</h4>', '', this.success);
                    })
                    .catch((error) => {
                        $blockUI.close()                        

                        if (error.response.status == 400) {
                            this.$alert('<h4>' + error.response.data + '</h4>', '', this.warning);
                        } else if (error.response.status == 404) {
                            this.$alert('<h4>' + error.response.data + '</h4>', '', this.error);
                        } else {
                            console.log(error.response);
                        }
                    });
                } else {
                    console.log("form not complete");
                    return false;
                }
            });
        },
        SetRules() {

            this.rules = {
                Email: [
                    { required: true, message: 'Please input your Email', trigger: 'blur' },
                    { type: 'email', message: 'Please input correct email address', trigger: ['blur', 'change'] }
                ],
                Password: [
                    { required: true, message: 'Please input your Password', trigger: 'blur' }
                ],
            }
        },
    }
}
