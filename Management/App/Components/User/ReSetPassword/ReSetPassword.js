export default {
    name: 'ReSetPassword',    
    created() {
    

    },
    data() {
        return {
            OldPassword: '',
            NewPassword: '',
            NewPassword2: ''
        };
    },
    methods: {



        submitForm() {
            
            console.log(this.ruleForm);
            this.$refs[formName].validate((valid) => {
                if (NewPassword == NewPassword2) {
                    if (valid) {
                        this.$blockUI.Start();
                        this.$http.ReSetPassword(this.OldPassword, this.NewPassword)
                            .then(response => {
                                this.$blockUI.Stop();
                                this.$parent.state = 0;
                                this.$message({
                                    type: 'success',
                                    dangerouslyUseHTMLString: true,
                                    message: '<strong>' + response.data + '</strong>'
                                });
                                this.$parent.GetUser();
                            })
                            .catch((error) => {
                                this.$blockUI.Stop();
                                if (error.response.status == 400) {
                                    this.$message({
                                        type: 'error',
                                        dangerouslyUseHTMLString: true,
                                        message: '<strong>' + error.response.data + '</strong>'
                                    });
                                    return;
                                } else if (error.response.status == 404) {
                                    this.$message({
                                        type: 'error',
                                        dangerouslyUseHTMLString: true,
                                        message: '<strong>' + error.response.data + '</strong>'
                                    });
                                    return;
                                } else {
                                    console.log(error.response);
                                }
                            });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                } else {
                    this.$message({
                        type: 'error',
                        dangerouslyUseHTMLString: true,
                        message: '<strong>' + + '</strong>'
                    });
                }
            });
        }
 



    }    
}
