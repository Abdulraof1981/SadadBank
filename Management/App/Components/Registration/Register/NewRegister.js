export default {
    name: 'NewRegister',    
    created() {
        this.GetAllCities();
    },
    data() {
        return {

            ruleForm: {
                Nid: '',
                Name: '',
                FatherName: '',
                GrandName: '',
                SurName: '',
                Gender: '',
                BirthDate: '',
                Email: '',
                Phone: '',
                CustomerName : '',
                Cityid: '',
                Address: '',
                PassportNumber: '',
                PassportExportDate: ''

            },
            rules: {
                Nid: [
                    { required: true, message: 'الرجاء ادخال الرقم الوطني', trigger: 'blur' },
                ],

                Phone: [
                    { required: true, message: 'الرجاء ادخال رقم الهاتف', trigger: 'blur' },
                ],

                Name: [
                    { required: true, message: 'الاسم اجباري', trigger: 'blur' },
                ],

                FatherName: [
                    { required: true, message: 'اسم الاب اجباري', trigger: 'blur' },
                ],

                SurName: [
                    { required: true, message: 'اللقب اجباري', trigger: 'blur' },
                ],

                Gender: [
                    { required: true, message: 'الجنس اجباري', trigger: 'blur' },
                ],

                BirthDate: [
                    { required: true, message: 'تاريخ الميلاد اجباري', trigger: 'blur' },
                ],

                CustomerName: [
                    { required: true, message: 'الرجاء ادخال رقم هاتف مستخدم الخدمة لكي يتم جلب اسم الزبون', trigger: 'blur' },
                ],

                Cityid: [
                    { required: true, message: 'الرجاء اختيار المدينة', trigger: 'change' },
                ],

                PassportNumber: [
                    { required: true, message: 'الرجاء ادخال رقم جواز السفر', trigger: 'blur' },
                ],

                PassportExportDate: [
                    { required: true, message: 'الرجاء ادخال تاريخ انتهاء صلاحية جواز السفر', trigger: 'blur' },
                ]
                                
            },

            AllCities : [],
          
        };
    },
    methods: {
        Back() {
            this.$parent.state = 0;
        },
        
        getNidInfo(nid) {
            if (nid === undefined || nid === null || nid === "") {
                return;
            }
            if (nid.length != 12) {
                this.$message({
                    type: 'info',
                    dangerouslyUseHTMLString: true,
                    message: '<strong>' + 'الرقم الوطني غير صحيح الرجاء المحاولة مرة أخرى' + '</strong>'
                });
                return;
            }

            this.$http.getNidInfo(nid)
                .then(response => {
                this.$blockUI.Stop();
                    if (response.data.code == 0) {
                        this.ruleForm.Name = response.data.name;
                        this.ruleForm.FatherName = response.data.fatherName;
                        this.ruleForm.GrandName = response.data.grandName;
                        this.ruleForm.SurName = response.data.surName;
                        this.ruleForm.Gender = ( response.data.gender == 1 ) ? "ذكر" : "أنثى";
                        this.ruleForm.BirthDate = this.formatDate(response.data.birthDate);

                        document.getElementById("NID").readOnly = true;
                        document.getElementById("NID").style.cursor = "not-allowed";
                        document.getElementById("NID").style.backgroundColor = "#eee";

                        document.getElementById("showBtn").disabled = true;
                    } else if (response.data.code == -1) {
                        this.$message({
                            type: 'error',
                            dangerouslyUseHTMLString: true,
                            message: '<strong>' + 'حدث خطا الرجاء المحاولة مرة أخرى' + '</strong>'
                        });
                    } else {
                        this.$message({
                            type: 'info',
                            dangerouslyUseHTMLString: true,
                            message: '<strong>' + 'الرقم الوطني غير صحيح الرجاء المحاولة مرة أخرى' + '</strong>'
                        });
                    }
            })
            .catch((err) => {
                this.$blockUI.Stop();
                console.error(err);
                this.pages = 0;
            });
        },

        GetAllCities() {
            this.$http.GetAllCities()
            .then(response => {
                this.AllCities = response.data.cities;
                console.log(this.AllCities[10].cityName);
            }).catch((err) => {
                console.error(err);
            });
        },
        
        resetNid() {
            this.ruleForm.Name = "";
            this.ruleForm.FatherName = "";
            this.ruleForm.GrandName = "";
            this.ruleForm.SurName = "";
            this.ruleForm.Nid = "";
            this.ruleForm.Gender = "";
            this.ruleForm.BirthDate = "";
            
            document.getElementById("NID").readOnly = false;
            document.getElementById("NID").style.cursor = "text";
            document.getElementById("NID").style.backgroundColor = "white";

            document.getElementById("showBtn").disabled = false;
        },

        formatDate(date) {
            var d = new Date(date),
                month = '' + (d.getMonth() + 1),
                day = '' + d.getDate(),
                year = d.getFullYear();

            if (month.length < 2) month = '0' + month;
            if (day.length < 2) day = '0' + day;

            return [year, month, day].join('-');
        },
        
        getPhoneInfo(phone) {
            if (phone === undefined || phone === null || phone === "") {
                return;
            }
            if (phone.length != 9) {
                this.$message({
                    type: 'info',
                    dangerouslyUseHTMLString: true,
                    message: '<strong>' + 'رقم الهاتف غير صحيح الرجاء المحاولة مرة أخرى 91XXXXXXXX' + '</strong>'
                });
                return;
            }

            this.$http.getPhoneInfo(phone)
                .then(response => {
                    this.$blockUI.Stop();
                    if (response.data.code == 0) {
                        this.ruleForm.CustomerName = response.data.name + " ( " + response.data.nid + " )";
                        
                        document.getElementById("Phone").readOnly = true;
                        document.getElementById("Phone").style.cursor = "not-allowed";
                        document.getElementById("Phone").style.backgroundColor = "#eee";

                        document.getElementById("showPhoneBtn").disabled = true;
                    } else if (response.data.code == -1) {
                        this.$message({
                            type: 'error',
                            dangerouslyUseHTMLString: true,
                            message: '<strong>' + 'حدث خطا الرجاء المحاولة مرة أخرى' + '</strong>'
                        });
                    } else {
                        this.$message({
                            type: 'info',
                            dangerouslyUseHTMLString: true,
                            message: '<strong>' + 'خطأ في بيانات الهاتف الرجاء المحاولة مرة أخرى' + '</strong>'
                        });
                    }
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.pages = 0;
                });
        },

        resetPhone() {
            this.ruleForm.CustomerName = "";
            this.ruleForm.Phone = "";

            document.getElementById("Phone").readOnly = false;
            document.getElementById("Phone").style.cursor = "text";
            document.getElementById("Phone").style.backgroundColor = "white";

            document.getElementById("showPhoneBtn").disabled = false;
        },
        
        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    console.log(this.ruleForm);
                    this.$blockUI.Start();
                    this.$http.AddCustomer(this.ruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.$parent.state = 0;
                            this.$message({
                                type: 'success',
                                dangerouslyUseHTMLString: true,
                                message: '<strong>' + response.data+'</strong>'
                            });
                            this.$parent.GetCustomers(1);
                        }).catch((error) => {
                            this.$blockUI.Stop();
                            if (error.response.status == 400) {
                                this.$alert(error.response.data, '', this.warning);
                            } else if (error.response.status == 404) {
                                this.$alert(error.response.data , '', this.error);
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
