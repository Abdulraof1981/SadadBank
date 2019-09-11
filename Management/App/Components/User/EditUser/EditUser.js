export default {
    name: 'EditUsers',    
    created() {
        this.GetAllBranch();
        this.GetUserData();
    },
    data() {
        return {
            Branchs: [],
            BranchId: '',
            IsHaveExtraPermisssions: false,
            Permissions:[],
            PermisssionObj:[{ id: 1, name: 'تسجيل زبون - مبدئي' }, { id: 2, name: 'تسجيل زبون - نهائي' }, { id: 3, name: 'تعبئة نقدية - مبدئي' }, { id: 4, name: 'تعبئة نقدية - نهائي' }],
            //ConfirmPassword:'',
            ruleForm: {
                LoginName: '',
                FullName: '',
                Email: '',
                DateOfBirth: '',
                Gender: '',
                //Password: '',
                userType: '',
                BranchId: '',
                Permissions: [],
                SavedPermissions:''
                
            },
            ExtraPermissions: [],
            TablePermissions: [],
            SavedPermissions:[],
            rules: {
                LoginName: [
                    { required: true, message: 'الرجاء ادخال اسم المستخدم', trigger: 'blur' },
                    { min: 3, max: 20, message: 'الطول يجب ان يكون من 3 الي 20', trigger: 'blur' },
                    { required: true, pattern: /^\S*$/, message: 'يجب ان لايحتوي  علي مسافات', trigger: 'blur' }
                ],
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
                ],
                /*Password: [
                    { required: true, message: 'الرجاء ادخال الرقم السري', trigger: 'change' },
                    { min: 6, max: 20, message: 'الطول يجب ان يكون من 6 الي 20', trigger: 'blur' },
                    { required: true, pattern: /^\S*$/ , message: 'يجب ان لايحتوي الرقم السري علي مسافات', trigger: 'blur' }
                    
                ],*/
                Permissions: [
                    { required: true, message: 'الرجاء اختيار الصلاحيات', trigger: 'change' }
                ]
            },
            BankId: '',
            SelecteUserType: '',
            bankAddObj: '',
            branchAddObj : ''
        };
    },
    methods: {
        GetUserData() {
            this.$blockUI.Start();
            this.$http.GetUserData(this.$parent.userIdForEdit)
                .then(response => {
                    this.$blockUI.Stop();
                    
                    this.SelecteUserType = response.data.user.userType;
                    
                    this.ruleForm.LoginName = response.data.user.loginName;
                    this.ruleForm.FullName = response.data.user.fullName;
                    this.ruleForm.Email = response.data.user.email;
                    this.ruleForm.DateOfBirth = response.data.user.dateOfBirth;
                    this.ruleForm.Gender = response.data.user.gender.toString();
                    
                    this.bankAddObj = response.data.user.bank;
                    this.branchAddObj = response.data.user.branch;

                    if (response.data.user.registerMaker == 1) {
                        this.ruleForm.Permissions.push(1);
                    }
                    if (response.data.user.registerChecker == 1) {
                        this.ruleForm.Permissions.push(2);
                    }
                    if (response.data.user.cashInMaker == 1) {
                        this.ruleForm.Permissions.push(3);
                    }
                    if (response.data.user.cashInChecker == 1) {
                        this.ruleForm.Permissions.push(4);
                    }
                    
                    for (var i = 0; i < response.data.user.savedPermissions.length; i++) { 
                        var element = response.data.user.savedPermissions[i];
                    
                        this.BranchId = element.branchId;
                        
                        if (element.registerMaker == 1) {
                            this.ExtraPermissions.push(1);
                        }
                        if (element.registerChecker == 1) {
                            this.ExtraPermissions.push(2);
                        }
                        if (element.cashInMaker == 1) {
                            this.ExtraPermissions.push(3);
                        }
                        if (element.cashInChecker == 1) {
                            this.ExtraPermissions.push(4);
                        }

                        this.AddExtraPermissions();
                    }
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                });
        },

        ShowSaveButton() {
            if (this.IsHaveExtraPermisssions) {
                this.IsHaveExtraPermisssions = false;
            } else {
                this.IsHaveExtraPermisssions = true;
            }
        },

        GetUserPermission(id, ExtraPermission) {
            ExtraPermission.forEach(function (element) {
                console.log(id + " " + element);
                if (id == element) {
                    console.log(true);
                    return true;
                }     
            });
            //return false;
        },

        FilterMorePerm() {
            this.ExtraPermissions = [];
        },
        
        AddExtraPermissions() {
            if (!this.BranchId) {
                this.$message({
                    type: 'error',
                    dangerouslyUseHTMLString: true,
                    message: '<strong>' + ' خطأ : الرجاء اختيار اسم الفرع ' + '</strong>'
                });
                return;
            }

            if (this.branchAddObj.branchId == this.BranchId) {
                this.$message({
                    type: 'error',
                    dangerouslyUseHTMLString: true,
                    message: '<strong>' + ' خطأ : لقد تم اختيار صلاحية الفرع سابقا ' + '</strong>'
                });

                this.BranchId = '';
                this.ExtraPermissions = [];
                return;
            }

            if (this.SavedPermissions.find(x => x.BranchId == this.BranchId)!=undefined) {
                this.$message({
                    type: 'error',
                    dangerouslyUseHTMLString: true,
                    message: '<strong>' + ' خطأ : لقد تم اختيار صلاحية الفرع سابقا ' + '</strong>'
                });

                this.BranchId = '';
                this.ExtraPermissions = [];
                return;
            }

            var obj = {
                PermissionId: this.SelecteUserType,
                BankName: this.bankAddObj.name,
                BranchId: this.Branchs.find(x => x.branchId === this.BranchId).branchId,
                BranchName: this.Branchs.find(x => x.branchId === this.BranchId).name,
                ExtraPer: this.ExtraPermissions
            };
            
            var obj2 = {
                //PermissionId: this.$parent.SelecteUserType,
              
                BranchId:this.BranchId,
                ExtrPer: this.ExtraPermissions,
            }
            
            this.TablePermissions.push(obj);
            this.SavedPermissions.push(obj2);

            this.BranchId = '';
            this.ExtraPermissions = [];
        },


        GetAllBranch() {
            this.$blockUI.Start();
            this.$http.GetAllBranchsByUserId(this.$parent.userIdForEdit)
                .then(response => {
                    this.$blockUI.Stop();
                    this.Branchs = response.data.branchs
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

        RemovePermission(branchId) {
            this.$confirm('هل انت متأكد من حذف هذه الصلاحية؟', 'تـحذير', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'لا',
                type: 'warning'
            }).then(() => {
                this.TablePermissions = this.TablePermissions.filter(x => x.BranchId !== branchId);
                this.SavedPermissions = this.SavedPermissions.filter(x => x.BranchId !== branchId);
            });
        },

        submitForm(formName) {
           
            //console.log(this.SavedPermissions);
            //console.log(this.TablePermissions);
            /*if (this.ConfirmPassword != this.ruleForm.Password) {
                this.$message({
                    type: 'error',
                    dangerouslyUseHTMLString: true,
                    message: '<strong>' + 'الرجاء التأكد من تطابق الرقم السري' + '</strong>'
                });
                return;
            }*/
            
            this.ruleForm.userType = this.SelecteUserType;
            this.ruleForm.BranchId = this.BranchId;
            this.ruleForm.SavedPermissions = this.SavedPermissions;
            
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.$blockUI.Start();
                    //console.log(this.ruleForm);

                    this.$http.EditUser(this.ruleForm)
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

        /*resetForm(formName) {
            this.TablePermissions = [];
            this.SavedPermissions = [];
            this.$refs[formName].resetFields();
        }*/



    }    
}
