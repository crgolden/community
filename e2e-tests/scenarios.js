'use strict';

// Angular E2E Testing Guide:
// https://docs.angularjs.org/guide/e2e-testing

describe('Community Application', function () {

    it('should redirect `#!/index.html` to `#!/', function () {
        browser.get('http://localhost:9052/#!/index.html');
        expect(browser.getLocationAbsUrl()).toBe('/');
    });

    describe('View: Event list', function () {

        beforeEach(function () {
            browser.get('#!/events');
        });

        it('should filter the event list as a user types into the search box', function () {
            var eventList = element.all(by.repeater('event in $ctrl.events'));
            var query = element(by.model("$ctrl.query"));

            expect(eventList.count()).toBe(20);

            query.sendKeys('nexus');
            expect(eventList.count()).toBe(1);

            query.clear();
            query.sendKeys('motorola');
            expect(eventList.count()).toBe(8);
        });

        it('should be possible to control event order via the drop-down menu', function () {
            var queryField = element(by.model('$ctrl.query'));
            var orderSelect = element(by.model('$ctrl.orderProp'));
            var nameOption = orderSelect.element(by.css('option[value="name"]'));
            var eventNameColumn = element.all(by.repeater('event in $ctrl.events').column('event.name'));

            function getNames() {
                return eventNameColumn.map(function (elem) {
                    return elem.getText();
                });
            }

            queryField.sendKeys('tablet');   // Let's narrow the dataset to make the assertions shorter

            expect(getNames()).toEqual([
              'Motorola XOOM with Wi-Fi',
              'MOTOROLA XOOM'
            ]);

            nameOption.click();

            expect(getNames()).toEqual([
              'MOTOROLA XOOM',
              'Motorola XOOM with Wi-Fi'
            ]);
        });

        it('should render event specific links', function () {
            var query = element(by.model('$ctrl.query'));
            query.sendKeys('nexus');

            element.all(by.css('.events li a')).first().click();
            expect(browser.getLocationAbsUrl()).toBe('/events/nexus-s');
        });

    });

    describe('View: Event detail', function () {

        beforeEach(function () {
            browser.get('#!/events/nexus-s');
        });

        it('should display the `nexus-s` page', function () {
            expect(element(by.binding('$ctrl.event.name')).getText()).toBe('Nexus S');
        });

        it('should display the first event image as the main event image', function () {
            var mainImage = element(by.css('img.event.selected'));

            expect(mainImage.getAttribute('src')).toMatch(/images\/events\/nexus-s.0.jpg/);
        });

        it('should swap the main image when clicking on a thumbnail image', function () {
            var mainImage = element(by.css('img.event.selected'));
            var thumbnails = element.all(by.css('.event-thumbs img'));

            thumbnails.get(2).click();
            expect(mainImage.getAttribute('src')).toMatch(/images\/events\/nexus-s.2.jpg/);

            thumbnails.get(0).click();
            expect(mainImage.getAttribute('src')).toMatch(/images\/events\/nexus-s.0.jpg/);
        });

    });

});
