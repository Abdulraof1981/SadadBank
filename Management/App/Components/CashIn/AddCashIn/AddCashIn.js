export default {
    name: 'AddCashIn',
    created() {

    },
    data() {
        return {

            ruleForm: {
                Valuedigits: '',
                NumInvoiceDep: '',
                description: '',
                personalId:''
            },
            rules: {
                Valuedigits: [
                    { required: true, message: 'الرجاء ادخال القيمة', trigger: 'blur' },
                    { required: true, pattern: /^\d+$/ , message: 'القيمة يجب ان تكون ارقام فقط', trigger: 'blur' },
                    //{ min: 50, max: 3000, message: 'اقصي قيمة يجب ان تكون 3000 د.ل و اقل قيمة يجب ان تكون 50د.ل', trigger: 'blur' }
                ],
                NumInvoiceDep: [
                    { required: true, message: 'الرجاء ادخال رقم الايصال', trigger: 'blur' },
                ],
                description: [
                    { required: true, message: 'الرجاء ادخال معلومات اخري', trigger: 'change' }
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
                    this.ruleForm.personalId = this.$parent.CitizenInfo.id;
                    this.$http.AddCashIn(this.ruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.$parent.state = 0;
                            this.$message({
                                type: 'success',
                                dangerouslyUseHTMLString: true,
                                message: '<strong>' + response.data + '</strong>'
                            });
                            this.$parent.Search();

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
