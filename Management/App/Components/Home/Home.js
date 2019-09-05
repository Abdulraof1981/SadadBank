

export default {
    name: 'home',
    components: {
      
    },
    created() { 
        var loginDetails = sessionStorage.getItem('currentUser');
        this.loginDetails = JSON.parse(loginDetails);
        if (loginDetails != null) {
            this.loginDetails = JSON.parse(loginDetails);
            this.open = true;

        } else {
            window.location.href = '/Security/Login';
        }
        this.GetStatistic();
        setInterval(() => this.GetStatistic() , 10000);   
    },
    data() {
        return {
            open: false,
            loginDetails: {},
            ST:''
        };
    },
    methods: {

        // GetStatistic
        GetStatistic() {
         //   this.$blockUI.Start();
            this.$http.GetStatistic()
                .then(response => {
                   // this.$blockUI.Stop();
                    this.ST = response.data;
                })
                .catch((err) => {
                  //  this.$blockUI.Stop();
                    console.error(err);
                    this.$message({
                        type: 'error',
                        dangerouslyUseHTMLString: true,
                        message: '<strong>' + err.response.data + '</strong>'
                    });
                    this.pages = 0;
                });
        }


    }    
}
