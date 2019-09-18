export default {
    name: 'ReSetPassword',    
    created() {
    

    },
    data() {
        return {

            ruleForm :{
               OldPassord: '',
              NewPassword: '',
               NewPassword2: ''

            },
            rules: {
                OldPassword: [
                    { required: true, message: 'الرجاء ادخل الرقم السري', trigger: 'change' },
                    { min: 6, max: 20, message: 'الطول يجب ان يكون من 6 الي 20', trigger: 'blur' },
                    { required: true, pattern: /^\S*$/, message: 'يجب ان لايحتوي الرقم السري علي مسافات', trigger: 'blur' }

                ],
                NewPassword: [
                    { required: true, message: 'الرجاء ادخال الرقم السري', trigger: 'change' },
                    { min: 6, max: 20, message: 'الطول يجب ان يكون من 6 الي 20', trigger: 'blur' },
                    { required: true, pattern: /^\S*$/, message: 'يجب ان لايحتوي الرقم السري علي مسافات', trigger: 'blur' }

                ],
                NewPassword2: [
                    { required: true, message: 'الرجاء ادخال الرقم السري', trigger: 'change' },
                    { min: 6, max: 20, message: 'الطول يجب ان يكون من 6 الي 20', trigger: 'blur' },
                    { required: true, pattern: /^\S*$/, message: 'يجب ان لايحتوي الرقم السري علي مسافات', trigger: 'blur' }

                ]
            }
         
        };
    },
    methods: {

        Back() {
            this.$parent.state = 0;
        },

        submitForm(formName) {
        //    debugger;
            console.log(this.ruleForm);
            if (this.ruleForm.NewPassword != this.ruleForm.NewPassword2) {
                this.$message({
                    type: 'error',
                    dangerouslyUseHTMLString: true,
                    message: '<strong>' + "كلمة المرور الجديدة غير متطابقة " + '</strong>'
                });
                return;
            }
          
            this.$refs[formName].validate((valid) => {
                console.log(valid);
                    if (valid) {
                        this.$blockUI.Start();
                        this.$http.ChangePassword(this.ruleForm.OldPassword, this.ruleForm.NewPassword)
                            .then(response => {
                                this.$blockUI.Stop();
                                console.log(response.data);
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
                                } else if (error.response.status == 402 || error.response.status == 401) {
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

                });
           
        }



    }    
}
