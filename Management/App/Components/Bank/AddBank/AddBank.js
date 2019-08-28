export default {
    name: 'AddBank',    
    created() {
       
    },
    data() {
        return {

            ruleForm: {
                name: '',
                description: '',
            },
            rules: {
                name: [
                    { required: true, message: 'الرجاء ادخال اسم المصرف', trigger: 'blur' },
                ],
                description: [
                    { required: true, message: 'الرجاء ادخال معلومات عن المصرف', trigger: 'change' }
                ],

            }
          
         
        };
    },
    methods: {
        Back() {
            this.$parent.state = 0;
        },

        submitForm(formName) {
        
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.$blockUI.Start();
                    this.$http.AddBank(this.ruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.$parent.state = 0;
                            this.$message({
                                type: 'success',
                                dangerouslyUseHTMLString: true,
                                message: '<strong>' + response.data+'</strong>'
                            });
                            this.$parent.GetBank();

                        })
                        .catch((error) => {
                            this.$blockUI.Stop();
                            if (error.response.status == 400) {
                                this.$alert('<h4>' + error.response.data + '</h4>', '', this.warning);
                            } else if (error.response.status == 404) {
                                this.$alert('<h4>' + error.response.data + '</h4>', '', this.error);
                            } else {
                                console.log(error.response);
                            }
                        });




                } else {
                    console.log('error submit!!');
                    return false;
                }
            });
        },
        resetForm(formName) {
            this.$refs[formName].resetFields();
        }



    }    
}
