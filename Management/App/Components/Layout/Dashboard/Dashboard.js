export default {
    name: 'appHeader',    
    created() { 
        var route = window.location.href.split("/")[3];
        console.log(route);
        this.pathChange(route);

        var loginDetails = sessionStorage.getItem('currentUser');
        this.loginDetails = JSON.parse(loginDetails);
        if (loginDetails != null) {
            this.loginDetails = JSON.parse(loginDetails);
            if (this.loginDetails.userType == 5) {
                this.ColorCode = '#933c3c';
            }
        } else {
            window.location.href = '/Security/Login';
        }
        
    },
    data() {
        return {            
            loginDetails: null,
            active:1
        };
    },
  
    methods: {
        pathChange(route) {
            if (route == "bank") {
                this.active = 2;
            } else if (route == "Branch") {
                this.active = 3;
            } else if (route == "User") {
                this.active = 7;
            } else if (route =="CashIn") { 
                this.active = 5;
            } else if (route == "CashInChecker") { 
                this.active = 6;
            } else {
                this.active = 1;
            }
        },

        href(url) {
            this.$router.push(url);
        },
        OpenDropDown() {
            var root = document.getElementById("DropDown");
            if (root.getAttribute('class') == 'dropdown') {
                root.setAttribute('class', 'dropdown open');
            } else {
                root.setAttribute('class', 'dropdown');
            }

        },

        // ********************** Template InterActive ***********
        OpenMenuByToggle() {
            var root = document.getElementsByTagName('html')[0]; // '0' to assign the first (and only `HTML` tag)
            if (root.getAttribute('class') == 'nav-open') {
                root.setAttribute('class', '');
            } else {
                root.setAttribute('class', 'nav-open');
            }
        },
        OpenNotificationMenu() {
            var root = document.getElementById("Notifications");
            if (root.getAttribute('class') == 'dropdown open') {
                root.setAttribute('class', 'dropdown');
            } else if (root.getAttribute('class') == 'dropdown') {
                root.setAttribute('class', 'dropdown open');
            }
        },
        //****************************************************************
          openProfile() {
            var root = document.getElementById("collapseExample");
            var prof = document.getElementById("profile");
            if (root.getAttribute('class') == 'collapse show') {
                prof.setAttribute('aria-expanded', false);
                root.setAttribute('class', 'collapse');

            } else {
                root.setAttribute('class', 'collapse show');
                prof.setAttribute('aria-expanded', true);

            }



        },
      
    }    
}
