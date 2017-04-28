import { HttpClient, json } from 'aurelia-fetch-client';
import { inject } from 'aurelia-framework';
import { DataPoint, SumpPump } from './models';

@inject(HttpClient)
export class Api {
    private fetchClient: HttpClient;

    constructor(http: HttpClient) {
        this.fetchClient = http;
        this.fetchClient.configure(config => {
            config
                .withBaseUrl('api/')
                .rejectErrorResponses()
                .withDefaults({
                    credentials: 'same-origin',
                    headers: {
                        'Accept': 'application/json',
                        'X-Requested-With': 'Fetch'
                    }
                });
        })
    }

    getDataPoints(pumpId: string): Promise<DataPoint[]> {

        return this.fetchClient.fetch('pumps/data/' + pumpId)
            .then(result => result.json() as Promise<DataPoint[]>)
            .then(data => {
                return data;
            });
    }

    getDataPointsByDate(pumpId: string, startDate: string, endDate: string): Promise<DataPoint[]> {

        return this.fetchClient.fetch('pumps/data/' + pumpId + '?startDate=' + startDate + '&endDate=' + endDate)
            .then(result => result.json() as Promise<DataPoint[]>)
            .then(data => {
                return data;
            });
    }

    getPumps(): Promise<SumpPump[]> {

        return this.fetchClient.fetch('pumps/')
            .then(result => result.json() as Promise<SumpPump[]>)
            .then(data => {
                return data;
            });
    }

    getPump(pumpId: string): Promise<SumpPump> {

        return this.fetchClient.fetch('pumps/' + pumpId)
            .then(result => result.json() as Promise<SumpPump>)
            .then(data => {
                return data;
            });
    }

    updatePump(pump: SumpPump) {
        this.fetchClient.fetch('pumps/' + pump.pumpId, {
                method: 'put',
                body: json(pump)
        })
        .then()
    }

}