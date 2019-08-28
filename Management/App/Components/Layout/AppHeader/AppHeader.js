export default {
    name: 'AppHeader',    
    created() { 
        this.userInfo = JSON.parse(sessionStorage.getItem('currentUser'));
        if (this.userInfo == null) {
            window.location.href = '/Security/Login';
        }
    },
    data() {
        return { 
            userInfo: null,
            loginDetails: null,
            active:1
        };
    },
  
    methods: {
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
        }
        //****************************************************************

      
    }    
}
