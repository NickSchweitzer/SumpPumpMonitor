import { Api } from '../../api';
import { inject, computedFrom } from 'aurelia-framework';
import { SumpPump, DataPoint } from '../../models';
import * as chartjs from 'chart.js';

@inject(Api)
export class PumpDetail {
    public pumpId: string;
    public pump: SumpPump;
    public data: DataPoint[];
    public startDate: string;
    public endDate: string;
    private apiClient: Api;

    constructor(api: Api) {
        this.apiClient = api;
    }

    @computedFrom('data')
    get timeScaleOptions(): chartjs.ChartOptions {
        return {
            scales: {
                xAxes: [{
                    type: 'time',
                    time: {
                        minUnit: 'second',
                        tooltipFormat: "h:mm:ss a MM-DD-YYYY",
                        max: this.data[0].timeStamp,
                        min: this.data[this.data.length - 1].timeStamp
                    }
                }]
            }
        };
    }

    @computedFrom('data', 'pump')
    get timeScaleData(): chartjs.LinearChartData {
        return {
            labels: ['Pump Level'],
            datasets: [
                {
                    data: this.data.map(point => {
                        return {
                            x: point.timeStamp,
                            y: point.waterLevel
                        };
                    }),
                    label: this.pump.name
                }]
        };
    }

    activate(params, routeConfig) {
        this.pumpId = params.id;

        return this.apiClient.getPump(this.pumpId)
            .then(data => {
                this.pump = data;
                this.startDate = this.pump.lastDataRecorded;
                return this.apiClient.getDataPoints(this.pumpId)
                    .then(data => {
                        this.data = data;
                    });
            });
    }
}