import { Aurelia } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { EventAggregator } from 'aurelia-event-aggregator';
import { inject } from 'aurelia-framework';
import { SumpPump } from '../../models';
import { PumpSelected, PumpUpdated } from '../../messages';

@inject(Router, EventAggregator)
export class NavMenu {
    public router: Router;
    public selectedPump: SumpPump;

    constructor(router: Router, ea: EventAggregator) {
        this.router = router;
        ea.subscribe(PumpSelected, msg => this.select(msg.pump));
    }

    select(pump: SumpPump) {
        this.selectedPump = pump;
        return true;
    }
}