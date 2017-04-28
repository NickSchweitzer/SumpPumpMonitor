import 'isomorphic-fetch';
import { Aurelia } from 'aurelia-framework';
import 'bootstrap/dist/css/bootstrap.css';
import 'eonasdan-bootstrap-datetimepicker/build/css/bootstrap-datetimepicker.min.css';
import 'bootstrap';
import 'eonasdan-bootstrap-datetimepicker';
declare const IS_DEV_BUILD: boolean; // The value is supplied by Webpack during the build

export function configure(aurelia: Aurelia) {

    aurelia.use.standardConfiguration()
        .plugin('aurelia-chart')
        .plugin('aurelia-i18n', (i18n) => {
            return i18n.setup({
                lng: 'en',
                fallbackLng: 'en'
            });
        })
        .plugin('aurelia-notification', config => {
            config.configure({
                translate: false,
                notifications: {
                    'success': 'humane-libnotify-success',
                    'error': 'humane-libnotify-error',
                    'info': 'humane-libnotify-info'
                }
            });
        })
        .plugin('aurelia-validation')
        .plugin('aurelia-bootstrap-datetimepicker', config => {
            config.options.showTodayButton = true;
        });

    if (IS_DEV_BUILD) {
        aurelia.use.developmentLogging();
    }

    aurelia.start().then(() => aurelia.setRoot('app/components/app/app'));
}