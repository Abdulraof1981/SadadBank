export default {
    name: 'AddUsers',    
    created() {
        this.GetAllBranch();
    },
    data() {
        return {
            Branchs: [],
            BranchId: '',
            IsHaveExtraPermisssions: false,
            Permissions:[],
            PermisssionObj:[{ id: 1, name: 'تسجيل زبون - مبدئي' }, { id: 2, name: 'تسجيل زبون - نهائي' }, { id: 3, name: 'تعبئة نقدية - مبدئي' }, { id: 4, name: 'تعبئة نقدية - نهائي' }],
            ConfirmPassword:'',
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
                Password: [
                    { required: true, message: 'الرجاء ادخال الرقم السري', trigger: 'change' },
                    { min: 6, max: 20, message: 'الطول يجب ان يكون من 6 الي 20', trigger: 'blur' },
                    { required: true, pattern: /^\S*$/ , message: 'يجب ان لايحتوي الرقم السري علي مسافات', trigger: 'blur' }
                    
                ],
                Permissions: [
                    { required: true, message: 'الرجاء اختيار الصلاحيات', trigger: 'change' }
                ]
            }
        };
    },
    methods: {

        ShowSaveButton() {
            console.log(this.IsHaveExtraPermisssions);
            if (this.IsHaveExtraPermisssions) {
                this.IsHaveExtraPermisssions = false;
            } else {
                this.IsHaveExtraPermisssions = true;
            }
            console.log(this.IsHaveExtraPermisssions);
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

        //ShowSaveButton() {
            
        //},


        AddExtraPermissions() {
            if (!this.BranchId) {
                this.$message({
                    type: 'error',
                    dangerouslyUseHTMLString: true,
                    message: '<strong>' + ' خطأ : الرجاء اختيار اسم الفرع ' + '</strong>'
                });

                return;
            }

            if (this.$parent.branchAddObj.branchId == this.BranchId) {
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
                PermissionId: this.$parent.SelecteUserType,
                BankName: this.$parent.BankAddObj.name,
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
            if (this.ConfirmPassword != this.ruleForm.Password) {
                this.$message({
                    type: 'error',
                    dangerouslyUseHTMLString: true,
                    message: '<strong>' + 'الرجاء التأكد من تطابق الرقم السري' + '</strong>'
                });
                return;
            }


            this.ruleForm.userType = this.$parent.SelecteUserType;
            this.ruleForm.BranchId = this.$parent.BranchId;
            this.ruleForm.SavedPermissions = this.SavedPermissions;
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

        resetForm(formName) {
            this.TablePermissions = [];
            this.SavedPermissions = [];
            this.$refs[formName].resetFields();
        }



    }    
}
