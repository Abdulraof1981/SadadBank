

export default {
    name: 'home',
    components: {
      
    },
    created() { 
        this.GetStatistic();
    },
    data() {
        return {
            ST:''
        };
    },
    methods: {

        // GetStatistic
        GetStatistic() {
            this.$blockUI.Start();
            this.$http.GetStatistic()
                .then(response => {
                    this.$blockUI.Stop();
                    this.ST = response.data;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
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
