import { makeAutoObservable } from 'mobx';

class PortfolioStore {
  title = 'My Portfolio';

  constructor() {
    makeAutoObservable(this);
  }

  setTitle(newTitle: string) {
    this.title = newTitle;
  }
}

const portfolioStore = new PortfolioStore();
export default portfolioStore;
