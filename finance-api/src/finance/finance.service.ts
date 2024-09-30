import { Injectable, HttpException, HttpStatus } from '@nestjs/common';
import axios from 'axios';
import { PriceCriteriaDto } from './dto/price-criteria.dto';

@Injectable()
export class FinanceService {
 

  async fetchDailyPrices(priceCriteriaDto: PriceCriteriaDto): Promise<any[]> {
    const { symbol, startDate, endDate } = priceCriteriaDto;

     const fromPeriod = Math.floor(new Date(priceCriteriaDto.startDate).getTime() / 1000);
     const toPeriod = Math.floor(new Date(priceCriteriaDto.endDate).getTime() / 1000);

    // API URL for Yahoo Finance historical data
    const url = `https://query1.finance.yahoo.com/v8/finance/chart/${symbol}`;

    const params = {
      period1: fromPeriod, // start date timestamp
      period2: toPeriod, // end date timestamp
      interval: '1d', // daily data
      events: 'history',
    };

    // Fetch the data from Yahoo Finance
    const response = await axios.get(url, { params });
    const result = response.data.chart.result[0];
    const timestamps = result.timestamp; // array of Unix timestamps
    const quotes = result.indicators.quote[0]; // contains open, high, low, close, volume arrays

    // Iterate over the timestamps and corresponding price data
    const dailyPrices = timestamps.map((timestamp, index) => {
      const date = new Date(timestamp * 1000); // Convert Unix timestamp to JS date
      return {
        date: date.toISOString().split('T')[0], // Format as YYYY-MM-DD
        open: quotes.open[index],
        high: quotes.high[index],
        low: quotes.low[index],
        close: quotes.close[index],
        volume: quotes.volume[index],
      };
    });

    return dailyPrices; // Return the array of daily prices
  }

 
}
