import Vue from 'vue';
import VueI18n from 'vue-i18n'
import VueRouter from 'vue-router';
import ElementUI from 'element-ui';
import Vuetify from 'vuetify';
import locale from 'element-ui/lib/locale/lang/en'
import BlockUIService from './Shared/BlockUIService.js';
import Layout from './Components/Layout/Layout.vue';
import Home from './Components/Home/Home.vue';
import Bank from './Components/Bank/Bank.vue';
import Branch from './Components/Branch/Branch.vue';
import User from './Components/User/User.vue';
import CashIn from './Components/CashIn/CashIn.vue';
import EditUsersProfile from './Components/User/EditUsersProfile/EditUsersProfile.vue';
import ReSetPassword from './Components/User/ReSetPassword/ReSetPassword.vue';
import Registration from './Components/Registration/Registration.vue';
import CashInChecker from './Components/CashInChecker/CashInChecker.vue';

import DataService from './Shared/DataService';
import messages from './i18n';



Vue.use(Vuetify)
Vue.use(VueI18n);
Vue.use(VueRouter);
Vue.use(ElementUI,{ locale });

Vue.config.productionTip = false;

Vue.prototype.$http = DataService;
Vue.prototype.$blockUI = BlockUIService;


export const eventBus = new Vue();

const i18n = new VueI18n({
    locale: 'ar', // set locale
    messages, // set locale messages
})

const router = new VueRouter({
    mode: 'history',
    base: __dirname,
    linkActiveClass: 'active',
    routes: [
        { path: '/', component: Home }, 
        { path: '/Bank', component: Bank }, 
        { path: '/Branch', component: Branch }, 
        { path: '/User', component: User }, 
        { path: '/CashIn', component: CashIn },
        { path: '/Registration', component: Registration },
        { path: '/CashInChecker', component: CashInChecker },
        { path: '/EditUsersProfile', component: EditUsersProfile },
        { path: '/ReSetPassword', component: ReSetPassword }
      
        ]

});

Vue.filter('toUpperCase', function (value) {
    if (!value) return '';
    return value.toUpperCase();
});

new Vue({
    i18n,
    router,
    render: h => {
        return h(Layout);
    }    
}).$mount('#app');
