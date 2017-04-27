﻿import { Api } from '../../api';
import { inject, NewInstance } from 'aurelia-framework';
import { ValidationRules, ValidationController } from 'aurelia-validation';
import { SumpPump } from '../../models';

@inject(Api, NewInstance.of(ValidationController))
export class PumpDetail {
    public pumpId: string;
    public pump: SumpPump;
    private apiClient: Api;
    private validation: ValidationController;

    constructor(api: Api, validation: ValidationController) {
        this.apiClient = api;
        this.validation = validation;
    }

    activate(params, routeConfig) {
        this.pumpId = params.id;

        return this.apiClient.getPump(this.pumpId)
            .then(data => {
                this.pump = data;

                ValidationRules
                    .ensure((sp: SumpPump) => sp.name)
                        .displayName('Sump Pump Name')
                        .required()
                    .ensure((sp: SumpPump) => sp.maxWaterLevel)
                        .displayName('Maximum Water Level')
                        .required()
                        .satisfiesRule('integerRange', 0, 15)
                    .ensure((sp: SumpPump) => sp.maxRunTimeNoChange)
                        .displayName('Maximum Time of No Water Level Change')
                        .required()
                        .satisfiesRule('integerRange', 0, 60)
                    .on(this.pump);
            });
    }

    save() {
        this.validation.validate()
            .then(result => {
                if (result.valid) {
                    this.apiClient.updatePump(this.pump);
                } else {

                }
            })
    }
}