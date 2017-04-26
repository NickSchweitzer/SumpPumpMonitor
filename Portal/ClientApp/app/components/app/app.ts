import { Aurelia } from 'aurelia-framework';
import { inject } from 'aurelia-framework';
import { Router, RouterConfiguration } from 'aurelia-router';
import { NavMenu } from '../navmenu/navmenu'

@inject(NavMenu)
export class App {
    public menu: NavMenu;

    constructor(navMenu: NavMenu) {
        this.menu = navMenu;
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
            title: 'Pump Detail'
        }]);
    }
}