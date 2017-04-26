import { Api } from '../../api';
import { EventAggregator } from 'aurelia-event-aggregator';
import { inject } from 'aurelia-framework';
import { SumpPump } from '../../models';
import { PumpSelected, PumpUpdated } from '../../messages';

@inject(Api, EventAggregator)
export class Home {
    public pumps: SumpPump[];
    public events: EventAggregator;
    public selectedId: string;

    private apiClient: Api;

    constructor(api: Api, ea : EventAggregator) {
        this.pumps = [];
        this.apiClient = api;
        this.events = ea;

        //ea.subscribe(PumpSelected, msg => this.select(msg.pump));
        ea.subscribe(PumpUpdated, msg => {
            let id = msg.pump.pumpId;
            let found = this.pumps.find(x => x.pumpId === id);
            Object.assign(found, msg.pump);
        });
    }

    created() {
        return this.apiClient.getPumps()
            .then(data => {
                this.pumps = data;
            });
    }

    select(pump: SumpPump) {
        this.selectedId = pump.pumpId;
        this.events.publish(new PumpSelected(pump));
        return true;
    }
}