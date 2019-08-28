
import Vue from 'vue'
import ElementUI from 'element-ui'
import locale from 'element-ui/lib/locale/lang/ar'

import { Alert } from 'element-ui';

import Login from './Login/Login.vue';
import LoginService from './Shared/LoginService';

Vue.use(ElementUI, { locale });
Vue.use(Alert);


Vue.config.productionTip = false;
Vue.prototype.$http = LoginService;


new Vue({
    render: h => {
        return h(Login);
    }
}).$mount('#app2');