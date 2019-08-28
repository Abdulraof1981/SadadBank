export default {
    name: 'AppFooter',    
    created() { 
        
    },
    data() {
        return {            
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
