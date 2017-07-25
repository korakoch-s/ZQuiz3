import { ZQuiz3.NgAppPage } from './app.po';

describe('zquiz3.ng-app App', () => {
  let page: ZQuiz3.NgAppPage;

  beforeEach(() => {
    page = new ZQuiz3.NgAppPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!');
  });
});
