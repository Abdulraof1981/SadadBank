export default {
    name: 'EditUsersProfile',    
    created() {
        this.GetAllBranch();
       
        var loginDetails = sessionStorage.getItem('currentUser');
        if (loginDetails != null) {
           
            this.loginDetails = JSON.parse(loginDetails);
            this.ruleForm.FullName = this.loginDetails.fullName;
            this.ruleForm.Phone = this.loginDetails.phone;
            this.ruleForm.LoginName = this.loginDetails.loginName;
            this.ruleForm.Email = this.loginDetails.email;
            this.ruleForm.Gender = this.loginDetails.gender;
            this.ruleForm.DateOfBirth = this.loginDetails.dateOfBirth;

   
            //this.form.OfficeName = this.loginDetails.officeName;
            this.ruleForm.userType = this.loginDetails.userType;
    
        } else {
            window.location.href = '/Security/Login';
        }
    },
    data() {
        return {
   
            ruleForm: {
                LoginName: '',
                FullName: '',
                Email: '',
                DateOfBirth: '',
                Gender: '',
                Password: '',
                userType: '',
                BranchId: '',
                Permissions: [],
                SavedPermissions: '',
          photo: []
               
            },
            photo: [],
            rules: {


       
                FullName: [
                    { required: true, message: 'الرجاء ادخال الاسم التلاثي', trigger: 'blur' },
                      { min: 3, max: 30, message: 'الطول يجب ان يكون من 3 الي 30', trigger: 'blur' },
                    //{ required: true, pattern: /^\S*$/, message: 'يجب ان لايحتوي  علي مسافات', trigger: 'blur' }
                ],
                Email: [
                    { required: true, message: 'الرجاء ادخال البريد الالكتروني', trigger: 'blur' },
                    { required: true, pattern: /^(([^<>()\[\]\.,;:\s@\"]+(\.[^<>()\[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i, message: 'الرجاء إدخال البريد الإلكتروني بطريقه الصحيحة', trigger: 'blur' }
                ],
                DateOfBirth: [
                    { required: true, message: 'الرجاء اختيار تاريخ الميلاد', trigger: 'change' }
                ],
                Gender: [
                    { required: true, message: 'الرجاء اختيار نوع الجنسية', trigger: 'change' }
                ]

        
             
            }
        };
    },
    methods: {

 



   

        //ShowSaveButton() {
            
        //},


 


        GetAllBranch() {
            this.$http.GetAllBranchsByBankId(this.$parent.BankId)
                .then(response => {
                    this.$blockUI.Stop();
                    this.Branchs = response.data.branchs;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.pages = 0;
                });
        },


        Back() {
            this.$parent.state = 0;
        },

        submitForm(formName) {
   


            this.ruleForm.userType = this.$parent.SelecteUserType;
            this.ruleForm.BranchId = this.$parent.BranchId;
           
            console.log(this.ruleForm);
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.$blockUI.Start();
                    this.$http.AddUser(this.ruleForm)
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
                                    message: '<strong>' + error.response.data  + '</strong>'
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
            });
        },
        FileChanged(e) {
            debugger;
            var files = e.target.files;

            if (files.length <= 0) {
                return;
            }

            if (files[0].type !== 'image/jpeg' && files[0].type !== 'image/png') {
                this.$message({
                    type: 'error',
                    message: 'عفوا يجب انت تكون الصورة من نوع JPG ,PNG'
                });
                this.photo = null;
                return;
            }

            var $this = this;

            var reader = new FileReader();
            reader.onload = function () {
                $this.photo = reader.result;
                $this.UploadImage();
            };
            reader.onerror = function (error) {
                $this.photo = null;
            };
            reader.readAsDataURL(files[0]);
        },
        UploadImage() {
            console.log(this.photo);
            console.log(this.loginDetails.userId);
            this.$blockUI.Start();
            var obj = {
                UserId: this.loginDetails.userId,
                Photo: this.photo               
            };
          
            this.$http.UploadImage(obj)
                .then(response => {
                    this.$blockUI.Stop();
                    this.$message({
                        type: 'info',
                        message: response.data
                    });

                    setTimeout(() =>
                        window.location.href = '/EditUsersProfile'
                        , 500);

                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.pages = 0;
                });
        }



    }    
}
