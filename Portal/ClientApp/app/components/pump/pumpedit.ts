import { Api } from '../../api';
import { inject, NewInstance } from 'aurelia-framework';
import { ValidationRules, ValidationController } from 'aurelia-validation';
import { SumpPump, PumpConfiguration } from '../../models';
import { Notification } from 'aurelia-notification';
//import "humane-js/themes/libnotify.css!";

@inject(Api, NewInstance.of(ValidationController), Notification)
export class PumpDetail {
    public pumpId: string;
    public pump: SumpPump;
    private apiClient: Api;
    private validation: ValidationController;
    private notify: Notification;

    constructor(api: Api, validation: ValidationController, notify: Notification) {
        this.apiClient = api;
        this.validation = validation;
        this.notify = notify;
    }

    activate(params, routeConfig) {
        this.pumpId = params.id;

        return this.apiClient.getPump(this.pumpId)
            .then(data => {
                this.pump = data;

                ValidationRules
                    .ensure((pc: PumpConfiguration) => pc.name)
                        .displayName('Sump Pump Name')
                        .required()
                    .ensure((pc: PumpConfiguration) => pc.maxWaterLevel)
                        .displayName('Maximum Water Level')
                        .required()
                        .satisfiesRule('integerRange', 0, 15)
                    .ensure((pc: PumpConfiguration) => pc.maxRunTimeNoChange)
                        .displayName('Maximum Time of No Water Level Change')
                        .required()
                        .satisfiesRule('integerRange', 0, 60)
                    .on(this.pump.desired);
            });
    }

    save() {
        this.validation.validate()
            .then(result => {
                if (result.valid) {
                    this.apiClient.updatePump(this.pump);
                    //this.notify.info('Sump Pump Details Saved');
                } else {

                }
            })
    }
}