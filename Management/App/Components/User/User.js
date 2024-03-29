﻿import addUser from './AddUser/AddUser.vue';
import editUser from './EditUser/EditUser.vue';
import moment from 'moment';
export default {
    name: 'Users',    
    created() {
       
        var loginDetails = sessionStorage.getItem('currentUser');
        this.loginDetails = JSON.parse(loginDetails);
        if (loginDetails != null) {
            this.loginDetails = JSON.parse(loginDetails);
            if (this.loginDetails.userType != 1 && this.loginDetails.userType != 2) {
                window.location.href = '/Security/Login';
            }
            if (this.loginDetails.userType == 1) {
                this.SelecteUserType = 1;
                this.UserTypeSelect = [{ id: 0, userType: 'الكل' }, { id: 1, userType: 'مدير النظام' }, { id: 2, userType: 'مدير المصرف' }, { id: 3, userType: 'موظف المصرف' }];
                this.GetUser(this.pageNo);
            } else if (this.loginDetails.userType == 2) {
                this.SelecteUserType = 2;
                this.UserTypeSelect = [{ id: 0, userType: 'الكل' }, { id: 2, userType: 'مدير المصرف' }, { id: 3, userType: 'موظف المصرف' }];
                this.GetUser(this.pageNo);
            } else {
                this.SelecteUserType = 2;
                this.UserTypeSelect = [];
            }

        } else {
            window.location.href = '/Security/Login';
        }
        

    },
    components: {
        'add-User': addUser,
        'edit-User': editUser
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
            loginDetails: {},
            UserTypeSelect: [],
            pageNo: 1,
            pageSize: 10,
            SelecteUserType:'',
            pages: 0,  
            Users: [],
            state: 0,
            UserType: '',
            BranchId: '',
            Banks: [],
            BankId: '',
            Branchs: [],
            BankAddObj: {},
            userIdForEdit: -1
            
            
        };
    },
    methods: {

        GetAllBank() {
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

        FilterBranchByBankId(event) {  
            this.BankAddObj = this.Banks.find(x => x.bankId === event);
            this.BranchId = '';
            this.GetAllBranchsByBankId();
        },

        FilterBranch(event) {
            this.branchAddObj = this.Branchs.find(x => x.branchId === event);
            console.log(this.branchAddObj);
        },

        FilterUserType() {
            this.BranchId = '';
            this.Branchs = [];  
            this.BankId = '';
            this.Banks = [];
            this.GetAllBank();
            this.GetUser(this.pageNo);
        },

        GetAllBranchsByBankId() {
            this.$http.GetAllBranchsByBankId(this.BankId)
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

        RemoveUser(userId) {
            this.$confirm('سيؤدي ذلك إلى حذف المستخدم نهائيا. استمر؟', 'تـحذير', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'لا',
                type: 'warning'
            }).then(() => {
                this.$blockUI.Start();
                this.$http.RemoveUser(userId)
                    .then(response => {
                        this.$blockUI.Stop();
                        this.GetUser(this.pageNo);
                        this.$message({
                            type: 'info',
                            dangerouslyUseHTMLString: true,
                            message: '<strong>' + 'تم مسح المستخدم بنجاح' + '</strong>'
                        });
                    })
                    .catch((err) => {
                        this.$blockUI.Stop();
                        console.error(err);
                        this.pages = 0;
                    });
            });
        },

        TextMessage(status) {
            console.log(status);
            if (status == 1) {
                return 'سيؤدي ذلك إلى تفعيل المستخدم . استمر؟';
            } else {
                return 'سيؤدي ذلك إلى الغاء تفعيل المستخدم . استمر؟';
            }
        },

        ActiveUser(status, UserId) {
            this.$confirm(this.TextMessage(status), 'تـحذير', {
                confirmButtonText: 'نـعم',
                cancelButtonText: 'لا',
                type: 'warning'
            }).then(() => {
                this.$blockUI.Start();
                if (status==1) {
                    var text = 'تم تفعيل المستخدم بنجاح';
                } else {
                    var text = 'تم الغاء التفعيل المستخدم بنجاح';
                }
               
                this.$http.ActivateUser(status, UserId)
                    .then(response => {
                        this.$blockUI.Stop();
                        this.GetUser(this.pageNo);
                        this.$message({
                            type: 'info',
                            dangerouslyUseHTMLString: true,
                            message: '<strong>' + text + '</strong>'
                        });
                    })
                    .catch((err) => {
                        this.$blockUI.Stop();
                        console.error(err);
                        this.pages = 0;
                    });
            });
        },

        GetUser(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }

            this.$blockUI.Start();
            this.$http.GetUsers(this.pageNo, this.pageSize, this.SelecteUserType,this.BranchId)
                .then(response => {
                    this.$blockUI.Stop();
                    this.Users = response.data.users;
                    this.pages = response.data.count;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    console.error(err);
                    this.pages = 0;
                });
        },


        AddUsers() {
            this.state = 1;
        },

        EditUser(userId) {
            this.userIdForEdit = userId;
            this.state = 2;
        }

        //RemoveBank(BankId) {
        //    this.$confirm('سيؤدي ذلك إلى حذف المصرف نهائيا. استمر؟', 'تـحذير', {
        //        confirmButtonText: 'نـعم',
        //        cancelButtonText: 'لا',
        //        type: 'warning'
        //    }).then(() => {
        //        this.$blockUI.Start();
        //        this.$http.RemoveBank(BankId)
        //            .then(response => {
        //                this.$blockUI.Stop();
        //                this.GetBank(this.pageNo);
        //                this.$message({
        //                    type: 'info',
        //                    dangerouslyUseHTMLString: true,
        //                    message: '<strong>' + 'تم مسح المصرف بنجاح' + '</strong>'
        //                });
        //            })
        //            .catch((err) => {
        //                this.$blockUI.Stop();
        //                console.error(err);
        //                this.pages = 0;
        //            });
        //    });
        //},   
        //AddBanks() {
        //    this.state = 1;
        //},
        //GetBank(pageNo) {
        //    this.pageNo = pageNo;
        //    if (this.pageNo === undefined) {
        //        this.pageNo = 1;
        //    }
        //    this.$blockUI.Start();
        //    this.$http.GetBank(this.pageNo, this.pageSize)
        //        .then(response => {
        //            this.$blockUI.Stop();
        //            this.Banks = response.data.banks;
        //            this.pages = response.data.count;
        //        })
        //        .catch((err) => {
        //            this.$blockUI.Stop();
        //            console.error(err);
        //            this.pages = 0;
        //        });
        //},


       
    }    
}
