import { Loading } from 'element-ui';
export default (function () {
    var blockService = null;
    return {
        $loading: null,
        Start() {
            blockService = this.$loading({
                fullscreen: true,
                text: 'loading ...'
            });
        },
        Stop() {
            blockService.close();
        }
    };
})();



