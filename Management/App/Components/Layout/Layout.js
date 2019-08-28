import AppHeader from './AppHeader/AppHeader.vue';
import AppFooter from './AppFooter/AppFooter.vue';
import Dashboard from './Dashboard/Dashboard.vue';

export default {
    name: 'layout',   
    components: {
        'app-header': AppHeader,
        'app-footer': AppFooter,
        'app-dasboard': Dashboard,
       
    },
    created() {
        this.$blockUI.$loading = this.$loading;
    },
    data() {
        return {
            isAuthenticated: false,
            isActive: false
        };
    },
    methods: {
      


    }
    
}
