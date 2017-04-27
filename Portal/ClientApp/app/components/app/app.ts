import { Aurelia } from 'aurelia-framework';
import { inject } from 'aurelia-framework';
import { Router, RouterConfiguration } from 'aurelia-router';
import { ValidationRules } from 'aurelia-validation';
import { NavMenu } from '../navmenu/navmenu'

@inject(NavMenu)
export class App {
    public menu: NavMenu;

    constructor(navMenu: NavMenu) {
        this.menu = navMenu;
        this.configureValidations();
    }

    configureRouter(config: RouterConfiguration, router: Router) {
        config.title = 'Sump Pump Monitor';
        config.map([{
            route: [ '', 'home' ],
            name: 'home',
            settings: { icon: 'home' },
            moduleId: '../home/home',
            nav: true,
            title: 'Home'
        }, {
            route: 'pump/:id/',
            name: 'pumpdetail',
            settings: { icon: 'edit' },
            moduleId: '../pump/pumpdetail',
            nav: false,
            title: 'Pump Data'
        }, {
            route: 'pump/edit/:id/',
            name: 'pumpedit',
            settings: { icon: 'edit' },
            moduleId: '../pump/pumpedit',
            nav: false,
            title: 'Edit Pump'
        }]);
    }

    configureValidations() {
        ValidationRules.customRule(
            'integerRange',
            (value, obj, min, max) => {
                var num = Number.parseInt(value);
                return num === null || num === undefined || (Number.isInteger(num) && num >= min && num <= max);
            },
            "${$displayName} must be an integer between ${$config.min} and ${$config.max}.",
            (min, max) => ({ min, max })
        );
    }
}